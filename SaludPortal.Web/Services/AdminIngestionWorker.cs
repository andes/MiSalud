using System.Net;
using Microsoft.Extensions.Options;

namespace SaludPortal.Web.Services;

/// <summary>
/// Single-consumer background worker that ships queued logs/telemetry to Admin
/// with exponential backoff for transient failures.
/// </summary>
public sealed class AdminIngestionWorker : BackgroundService
{
    private readonly AdminIngestionQueue _queue;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AdminIngestionOptions _options;
    private readonly ILogger _logger;

    private long _discardedCount;
    private long _lastDiscardLogTicks;
    private static readonly long DiscardLogIntervalTicks = TimeSpan.FromSeconds(30).Ticks;

    public AdminIngestionWorker(
        AdminIngestionQueue queue,
        IHttpClientFactory httpClientFactory,
        IOptions<AdminIngestionOptions> options,
        ILoggerFactory loggerFactory)
    {
        _queue = queue;
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
        // Category blocked in ApiLogger so shipper failures never re-enter the queue.
        _logger = loggerFactory.CreateLogger(AdminIngestionQueue.LogCategory);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await foreach (var item in _queue.Reader.ReadAllAsync(stoppingToken))
            {
                LogQueueDropsIfNeeded();
                await ProcessItemAsync(item, stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Normal shutdown
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // Cancel ExecuteAsync first, then drain whatever remains with a short timeout.
        await base.StopAsync(cancellationToken);
        _queue.Complete();

        using var drainCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        drainCts.CancelAfter(TimeSpan.FromSeconds(Math.Max(1, _options.DrainTimeoutSeconds)));

        try
        {
            while (_queue.Reader.TryRead(out var item))
            {
                await ProcessItemAsync(item, drainCts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            var remaining = _queue.Reader.Count;
            if (remaining > 0)
            {
                _logger.LogWarning(
                    "Apagado: se abandonaron {Count} evento(s) pendientes en la cola de ingestión.",
                    remaining);
            }
        }
    }

    private AdminApiClient CreateClient()
        => new(_httpClientFactory.CreateClient(nameof(AdminApiClient)));

    private async Task ProcessItemAsync(IngestionItem item, CancellationToken cancellationToken)
    {
        var client = CreateClient();

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            item.Attempts++;

            try
            {
                using var response = item.Kind switch
                {
                    IngestionKind.Log => await client.SendLogAsync(item.Log!, cancellationToken),
                    IngestionKind.Telemetry => await client.SendTelemetryAsync(item.Telemetry!, cancellationToken),
                    _ => throw new InvalidOperationException($"Tipo de ingestión desconocido: {item.Kind}")
                };

                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                if (IsPermanentFailure(response.StatusCode))
                {
                    RecordDiscard(
                        $"Descartado por respuesta permanente {(int)response.StatusCode} ({item.Kind}).");
                    return;
                }

                if (item.Attempts >= _options.MaxAttempts)
                {
                    RecordDiscard(
                        $"Descartado tras {_options.MaxAttempts} intentos ({item.Kind}, HTTP {(int)response.StatusCode}).");
                    return;
                }

                var delay = ResolveRetryDelay(response, item.Attempts);
                _logger.LogDebug(
                    "Reintento {Attempt}/{Max} para {Kind} tras HTTP {Status} en {DelayMs} ms.",
                    item.Attempts,
                    _options.MaxAttempts,
                    item.Kind,
                    (int)response.StatusCode,
                    delay.TotalMilliseconds);

                await Task.Delay(delay, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (item.Attempts >= _options.MaxAttempts)
                {
                    RecordDiscard(
                        $"Descartado tras {_options.MaxAttempts} intentos ({item.Kind}): {ex.GetType().Name}: {ex.Message}");
                    return;
                }

                var delay = ComputeBackoff(item.Attempts);
                _logger.LogDebug(
                    ex,
                    "Reintento {Attempt}/{Max} para {Kind} tras error de red en {DelayMs} ms.",
                    item.Attempts,
                    _options.MaxAttempts,
                    item.Kind,
                    delay.TotalMilliseconds);

                await Task.Delay(delay, cancellationToken);
            }
        }
    }

    private static bool IsPermanentFailure(HttpStatusCode statusCode)
        => statusCode is HttpStatusCode.BadRequest
            or HttpStatusCode.Unauthorized
            or HttpStatusCode.Forbidden
            or HttpStatusCode.RequestEntityTooLarge
            or HttpStatusCode.NotFound;

    private TimeSpan ResolveRetryDelay(HttpResponseMessage response, int attempts)
    {
        if (response.StatusCode == HttpStatusCode.TooManyRequests
            && response.Headers.RetryAfter?.Delta is { } retryAfter
            && retryAfter > TimeSpan.Zero)
        {
            var capped = TimeSpan.FromMilliseconds(_options.MaxRetryDelayMs);
            return retryAfter < capped ? retryAfter : capped;
        }

        return ComputeBackoff(attempts);
    }

    private TimeSpan ComputeBackoff(int attempts)
    {
        var initial = Math.Max(1, _options.InitialRetryDelayMs);
        var max = Math.Max(initial, _options.MaxRetryDelayMs);
        // attempts is 1-based; delay grows after each failure
        var exp = Math.Min(attempts - 1, 16);
        var delayMs = (long)initial << exp;
        if (delayMs > max || delayMs < 0)
        {
            delayMs = max;
        }

        return TimeSpan.FromMilliseconds(delayMs);
    }

    private void LogQueueDropsIfNeeded()
    {
        var warning = _queue.TryConsumeDropWarning();
        if (warning is not null)
        {
            _logger.LogWarning("{Message}", warning);
        }
    }

    private void RecordDiscard(string detail)
    {
        Interlocked.Increment(ref _discardedCount);

        var now = DateTime.UtcNow.Ticks;
        var last = Interlocked.Read(ref _lastDiscardLogTicks);
        if (last == 0)
        {
            Interlocked.CompareExchange(ref _lastDiscardLogTicks, now, 0);
            _logger.LogWarning("Fallo de ingestión hacia Admin: {Detail}", detail);
            return;
        }

        if (now - last < DiscardLogIntervalTicks)
        {
            return;
        }

        if (Interlocked.CompareExchange(ref _lastDiscardLogTicks, now, last) != last)
        {
            return;
        }

        var count = Interlocked.Exchange(ref _discardedCount, 0);
        _logger.LogWarning(
            "Ingestión Admin: {Count} evento(s) descartado(s) en los últimos 30 s. Último: {Detail}",
            count,
            detail);
    }
}
