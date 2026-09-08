namespace SaludPortal.Web.Services;

/// <summary>
/// Logger provider that enqueues log entries for shipping to SaludPortal.Admin.
/// Client context (geo, device) is read from <see cref="BrowserContextCache"/>
/// populated by TelemetryService in the Blazor circuit context.
/// </summary>
public sealed class ApiLoggerProvider : ILoggerProvider
{
    private readonly AdminIngestionQueue _queue;
    private readonly BrowserContextCache _browserContextCache;

    public ApiLoggerProvider(
        AdminIngestionQueue queue,
        BrowserContextCache browserContextCache)
    {
        _queue = queue;
        _browserContextCache = browserContextCache;
    }

    public ILogger CreateLogger(string categoryName)
        => new ApiLogger(categoryName, _queue, _browserContextCache);

    public void Dispose() { }
}
