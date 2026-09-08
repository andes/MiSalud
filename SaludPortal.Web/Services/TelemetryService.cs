using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace SaludPortal.Web.Services;

public class TelemetryService
{
    private const string Source = "SaludPortal.Web";

    private readonly AdminIngestionQueue? _queue;
    private readonly NavigationManager _navigationManager;
    private readonly ClientContextService _clientContextService;
    private readonly BrowserContextCache _browserContextCache;

    private string? _lastTrackedRoute;

    public TelemetryService(
        IServiceProvider services,
        NavigationManager navigationManager,
        ClientContextService clientContextService,
        BrowserContextCache browserContextCache)
    {
        // Queue is only registered when AdminLogs:ApiKey is configured.
        _queue = services.GetService<AdminIngestionQueue>();
        _navigationManager = navigationManager;
        _clientContextService = clientContextService;
        _browserContextCache = browserContextCache;
    }

    public async Task TrackPageViewAsync(string? route = null)
    {
        var normalizedRoute = NormalizeRoute(route);
        if (normalizedRoute == _lastTrackedRoute)
        {
            return;
        }

        _lastTrackedRoute = normalizedRoute;
        await TrackAsync("view_page", normalizedRoute, null, null);
    }

    public Task TrackClickAsync(string target, string? route = null, object? metadata = null)
        => TrackAsync("click", NormalizeRoute(route), target, metadata);

    public Task TrackLaboratorioPdfDownloadAsync(string laboratorioId)
        => TrackAsync("download_pdf", NormalizeRoute(null), "laboratorio/pdf", new { laboratorioId });

    public Task TrackPrestacionPdfDownloadAsync(string prestacionId, string tipoPrestacion)
        => TrackAsync("download_pdf", NormalizeRoute(null), "prestacion/pdf", new { prestacionId, tipoPrestacion });

    public Task TrackPrestacionViewImageAsync(string prestacionId, string tipoPrestacion)
        => TrackAsync("view_image", NormalizeRoute(null), "prestacion/imagen", new { prestacionId, tipoPrestacion });

    public Task TrackPrestacionCopyLinkAsync(string prestacionId, string tipoPrestacion)
        => TrackAsync("copy_link", NormalizeRoute(null), "prestacion/enlace", new { prestacionId, tipoPrestacion });

    private async Task TrackAsync(string eventType, string route, string? target, object? metadata)
    {
        if (_queue is null)
        {
            return;
        }

        var metadataJson = SerializeMetadata(metadata);
        var clientContext = await _clientContextService.BuildClientContextPayloadAsync();
        _browserContextCache.Update(clientContext);

        _queue.TryEnqueue(new IngestionItem
        {
            Kind = IngestionKind.Telemetry,
            Telemetry = new TelemetryDto(
                eventType,
                route,
                target,
                clientContext?.PatientId,
                clientContext?.SessionId,
                Source,
                metadataJson,
                clientContext?.ClientIp,
                clientContext?.Latitude,
                clientContext?.Longitude,
                clientContext?.UserAgent,
                clientContext?.Browser,
                clientContext?.OsName,
                clientContext?.OsVersion,
                clientContext?.DeviceType)
        });
    }

    private string NormalizeRoute(string? route)
    {
        var value = string.IsNullOrWhiteSpace(route) ? _navigationManager.Uri : route;

        // If already a relative path, return as-is
        if (!value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return value.StartsWith('/') ? value : "/" + value;
        }

        var relativeRoute = _navigationManager.ToBaseRelativePath(value);
        return string.IsNullOrWhiteSpace(relativeRoute) ? "/" :
               relativeRoute.StartsWith('/') ? relativeRoute : "/" + relativeRoute;
    }

    private static string? SerializeMetadata(object? metadata)
    {
        if (metadata is null)
        {
            return null;
        }

        var json = JsonSerializer.Serialize(metadata);
        return json.Length <= 4000 ? json : null;
    }
}
