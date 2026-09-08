using System.Threading.Channels;

namespace SaludPortal.Web.Services;

public enum IngestionKind
{
    Log,
    Telemetry
}

public sealed class IngestionItem
{
    public required IngestionKind Kind { get; init; }
    public LogIngestionDto? Log { get; init; }
    public TelemetryDto? Telemetry { get; init; }
    public int Attempts { get; set; }
}

/// <summary>
/// Bounded in-memory queue for Admin log/telemetry ingestion.
/// Full queue drops the oldest item so producers never block.
/// </summary>
public sealed class AdminIngestionQueue
{
    public const string LogCategory = "SaludPortal.Web.Ingestion";

    private readonly Channel<IngestionItem> _channel;
    private readonly int _capacity;
    private long _droppedCount;
    private long _lastDropLogTicks;
    private static readonly long DropLogIntervalTicks = TimeSpan.FromSeconds(30).Ticks;

    public AdminIngestionQueue(Microsoft.Extensions.Options.IOptions<AdminIngestionOptions> options)
    {
        _capacity = Math.Max(1, options.Value.QueueCapacity);
        _channel = Channel.CreateBounded<IngestionItem>(new BoundedChannelOptions(_capacity)
        {
            FullMode = BoundedChannelFullMode.DropOldest,
            SingleReader = true,
            SingleWriter = false
        });
    }

    public ChannelReader<IngestionItem> Reader => _channel.Reader;

    public long DroppedCount => Interlocked.Read(ref _droppedCount);

    /// <summary>
    /// Non-blocking enqueue. Returns false only if the channel is completed.
    /// When at capacity, DropOldest discards the oldest pending item.
    /// </summary>
    public bool TryEnqueue(IngestionItem item)
    {
        var wasFull = _channel.Reader.Count >= _capacity;
        if (!_channel.Writer.TryWrite(item))
        {
            RecordDrop();
            return false;
        }

        if (wasFull)
        {
            RecordDrop();
        }

        return true;
    }

    public void Complete() => _channel.Writer.TryComplete();

    private void RecordDrop()
    {
        Interlocked.Increment(ref _droppedCount);
    }

    /// <summary>
    /// Returns a throttled warning message when drops occurred, or null if none / still in cooldown.
    /// </summary>
    public string? TryConsumeDropWarning()
    {
        var dropped = Interlocked.Read(ref _droppedCount);
        if (dropped == 0)
        {
            return null;
        }

        var now = DateTime.UtcNow.Ticks;
        var last = Interlocked.Read(ref _lastDropLogTicks);
        if (last != 0 && now - last < DropLogIntervalTicks)
        {
            return null;
        }

        if (Interlocked.CompareExchange(ref _lastDropLogTicks, now, last) != last)
        {
            return null;
        }

        var count = Interlocked.Exchange(ref _droppedCount, 0);
        if (count == 0)
        {
            return null;
        }

        return $"Cola de ingestión llena: se descartaron {count} evento(s) (capacidad {_capacity}).";
    }
}
