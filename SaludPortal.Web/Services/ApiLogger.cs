namespace SaludPortal.Web.Services;

public sealed class ApiLogger : ILogger
{
    private readonly string _categoryName;
    private readonly AdminIngestionQueue _queue;
    private readonly BrowserContextCache _browserContextCache;

    public ApiLogger(
        string categoryName,
        AdminIngestionQueue queue,
        BrowserContextCache browserContextCache)
    {
        _categoryName = categoryName;
        _queue = queue;
        _browserContextCache = browserContextCache;
    }

    // Categories that must never feed back into the HTTP log shipper.
    // Logging these would cause an infinite loop: log → HTTP call → log → ...
    private static readonly string[] _blockedPrefixes =
    [
        "System.Net.Http",
        "Polly",
        "Microsoft.Extensions.Http",
        AdminIngestionQueue.LogCategory,
    ];

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel)
    {
        if (logLevel < LogLevel.Information) return false;
        foreach (var prefix in _blockedPrefixes)
            if (_categoryName.StartsWith(prefix, StringComparison.Ordinal)) return false;
        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var message = formatter(state, exception);
        var exceptionText = exception?.ToString();

        var clientContext = _browserContextCache.Current;

        _ = Task.Run(() =>
        {
            try
            {
                _queue.TryEnqueue(new IngestionItem
                {
                    Kind = IngestionKind.Log,
                    Log = new LogIngestionDto(
                        logLevel.ToString(),
                        _categoryName,
                        message,
                        exceptionText,
                        clientContext?.ClientIp,
                        clientContext?.Latitude,
                        clientContext?.Longitude,
                        clientContext?.UserAgent,
                        clientContext?.Browser,
                        clientContext?.OsName,
                        clientContext?.OsVersion,
                        clientContext?.DeviceType,
                        clientContext?.PatientId,
                        clientContext?.SessionId)
                });
            }
            catch
            {
                // Never throw from inside a logger
            }
        });
    }
}
