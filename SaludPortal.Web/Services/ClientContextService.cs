using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;

namespace SaludPortal.Web.Services;

public sealed record ClientContextPayload(
    string? ClientIp,
    double? Latitude,
    double? Longitude,
    string? UserAgent,
    string? Browser,
    string? OsName,
    string? OsVersion,
    string? DeviceType,
    string? PatientId,
    string? SessionId);

public class ClientContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJSRuntime _jsRuntime;

    private Task<DeviceContextJsModel?>? _deviceContextTask;
    private Task<GeoContextJsModel?>? _geoContextTask;
    private readonly UserContext _userContext;
    public string SessionId =>
        _httpContextAccessor.HttpContext?.User?.GetSessionId() ?? "";

    public ClientContextService(IHttpContextAccessor httpContextAccessor, IJSRuntime jsRuntime, UserContext userContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _jsRuntime = jsRuntime;
        _userContext = userContext;
    }

    public async Task<ClientContextPayload> BuildClientContextPayloadAsync()
    {
        var patientId = await _userContext.GetPacienteIdAsync();
        var device = await GetDeviceContextAsync();
        var geo = await GetGeoContextAsync();

        return new ClientContextPayload(
            GetClientIp(),
            geo?.Latitude,
            geo?.Longitude,
            device?.UserAgent,
            device?.Browser,
            device?.OsName,
            device?.OsVersion,
            device?.DeviceType,
            patientId,
            SessionId);
    }

    private async Task<DeviceContextJsModel?> GetDeviceContextAsync()
    {
        _deviceContextTask ??= TryInvokeAsync<DeviceContextJsModel>("saludClientContext.getDeviceContext");
        return await _deviceContextTask;
    }

    private async Task<GeoContextJsModel?> GetGeoContextAsync()
    {
        _geoContextTask ??= TryInvokeAsync<GeoContextJsModel>("saludClientContext.tryGetGeolocation");
        return await _geoContextTask;
    }

    private async Task<T?> TryInvokeAsync<T>(string identifier) where T : class
    {
        try
        {
            return await _jsRuntime.InvokeAsync<T?>(identifier);
        }
        catch (JSException)
        {
            return null;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    private string? GetClientIp()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context is null)
        {
            return null;
        }

        var forwardedFor = context.Request.Headers["X-Forwarded-For"].ToString();
        if (!string.IsNullOrWhiteSpace(forwardedFor))
        {
            var first = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(first))
            {
                return first;
            }
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }

    private sealed class DeviceContextJsModel
    {
        public string? UserAgent { get; set; }
        public string? Browser { get; set; }
        public string? OsName { get; set; }
        public string? OsVersion { get; set; }
        public string? DeviceType { get; set; }
    }

    private sealed class GeoContextJsModel
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
