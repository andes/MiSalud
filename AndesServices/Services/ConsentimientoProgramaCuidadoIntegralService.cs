using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AndesServices.DTOs;
using AndesServices.Interfaces;
using Microsoft.Extensions.Logging;

namespace AndesServices.Services;

public class ConsentimientoProgramaCuidadoIntegralService : IConsentimientoProgramaCuidadoIntegral
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ConsentimientoProgramaCuidadoIntegralService> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ConsentimientoProgramaCuidadoIntegralService(
        IHttpClientFactory httpClientFactory,
        ILogger<ConsentimientoProgramaCuidadoIntegralService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<List<ConsentimientoDto>> ObtenerConsentimientosAsync(
        string pacienteId,
        string? programa = null,
        int? version = null,
        CancellationToken ct = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Andes");
            var url = BuildConsentimientoUrl(pacienteId, programa, version);
            var response = await client.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();

            var consentimientos = await response.Content.ReadFromJsonAsync<List<ConsentimientoDto>>(JsonOptions, ct);
            return consentimientos ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener consentimientos para paciente {PacienteId}", pacienteId);
            return [];
        }
    }

    public async Task<bool> ValidarPacienteAsync(string documento, string sexo, CancellationToken ct = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Andes");
            var body = new ValidarPacienteRequest { Documento = documento, Sexo = sexo };
            var json = JsonSerializer.Serialize(body, JsonOptions);

            var request = new HttpRequestMessage(HttpMethod.Get, "core/tm/validarpaciente")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ValidarPacienteResponse>(JsonOptions, ct);
            return result?.Validado ?? false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar paciente documento {Documento}", documento);
            return false;
        }
    }

    public async Task<ConsentVersionDto?> ObtenerVersionProgramaAsync(string programa, CancellationToken ct = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Andes");
            var url = $"core/tm/consentimientoVersion?programa={Uri.EscapeDataString(programa)}&activo=true";
            var response = await client.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();

            var versiones = await response.Content.ReadFromJsonAsync<List<ConsentVersionDto>>(JsonOptions, ct);
            return versiones?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener versión del programa {Programa}", programa);
            return null;
        }
    }

    public async Task<ConsentimientoDto?> GuardarConsentimientoAsync(
        string programa,
        int version,
        string pacienteId,
        bool aceptacion,
        CancellationToken ct = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Andes");
            var body = new GuardarConsentimientoRequest
            {
                Programa = programa,
                Version = version,
                PacienteId = pacienteId,
                Aceptacion = aceptacion
            };

            var response = await client.PostAsJsonAsync("core/tm/consentimiento", body, JsonOptions, ct);
            response.EnsureSuccessStatusCode();

            var consentimientos = await response.Content.ReadFromJsonAsync<List<ConsentimientoDto>>(JsonOptions, ct);
            return consentimientos?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar consentimiento para paciente {PacienteId}, programa {Programa}", pacienteId, programa);
            return null;
        }
    }

    private static string BuildConsentimientoUrl(string pacienteId, string? programa, int? version)
    {
        var url = new StringBuilder("core/tm/consentimiento?pacienteId=")
            .Append(Uri.EscapeDataString(pacienteId));

        if (!string.IsNullOrWhiteSpace(programa))
        {
            url.Append("&programa=").Append(Uri.EscapeDataString(programa));
        }

        if (version.HasValue)
        {
            url.Append("&version=").Append(version.Value);
        }

        return url.ToString();
    }
}
