using AndesServices.DTOs.CentrosDeSalud;
using AndesServices.Interfaces;
using Newtonsoft.Json;

namespace AndesServices.Services;

public class CentrosSaludService : ICentrosSalud
{
    private readonly HttpClient _andesClient;
    private readonly ILogger<CentrosSaludService> _logger;

    public CentrosSaludService(IHttpClientFactory httpClientFactory, ILogger<CentrosSaludService> logger)
    {
        _andesClient = httpClientFactory.CreateClient("Andes");
        _logger = logger;
    }

    public async Task<List<CentroSaludAraucania>> ObtenerCentrosDeSaludAraucania()
    {
        try
        {
            using HttpResponseMessage response = await _andesClient.GetAsync("core/tm/areaAraucania");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                List<CentroSaludAraucania>? centros = JsonConvert.DeserializeObject<List<CentroSaludAraucania>>(responseBody);

                if (centros == null)
                {
                    return new List<CentroSaludAraucania>();
                }

                return centros;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los centros de salud en Araucania.");
            return new List<CentroSaludAraucania>();
        }

        return new List<CentroSaludAraucania>();
    }

    public async Task<List<CentroSaludProvincia>> ObtenerCentrosDeSaludProvincia()
    {
        try
        {
            using HttpResponseMessage response = await _andesClient.GetAsync("core/tm/organizaciones?showMapa=false");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                List<CentroSaludProvincia>? centros = JsonConvert.DeserializeObject<List<CentroSaludProvincia>>(responseBody);
                if (centros == null)
                {
                    return new List<CentroSaludProvincia>();
                }
                return centros;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los centros de salud en la provincia.");
            return new List<CentroSaludProvincia>();
        }
        return new List<CentroSaludProvincia>();
    }
}
