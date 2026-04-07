using AndesServices.DTOs.CentrosDeSalud;
using AndesServices.Interfaces;
using Newtonsoft.Json;

namespace AndesServices.Services;

public class CentrosSaludService : ICentrosSalud
{
    private readonly HttpClient _httpClient;

    public CentrosSaludService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Andes");
    }

    public async Task<List<CentroSaludAraucania>> ObtenerCentrosDeSaludAraucania()
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync("core/tm/areaAraucania");

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
            return new List<CentroSaludAraucania>();
        }

        return new List<CentroSaludAraucania>();
    }

    public async Task<List<CentroSaludProvincia>> ObtenerCentrosDeSaludProvincia()
    {
        try
        {
            using HttpResponseMessage response = await _httpClient.GetAsync("core/tm/organizaciones?showMapa=false");
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
            return new List<CentroSaludProvincia>();
        }
        return new List<CentroSaludProvincia>();
    }
}
