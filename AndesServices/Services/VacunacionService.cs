using AndesServices.Interfaces;
using SaludPortal.Application.DTOs.Vacunaciones;

namespace AndesServices.Services
{
    public class VacunacionService : IVacunacion
    {
        private readonly ILogger<VacunacionService> _logger;
        private readonly HttpClient _andesClient;

        public VacunacionService(IHttpClientFactory httpClientFactory, ILogger<VacunacionService> logger)
        {
            _logger = logger;
            _andesClient = httpClientFactory.CreateClient("Andes");
        }

        public Task<bool> ActualizarVacunacionAsync(string idVacunacion, string vacuna, string fechaVacuna, string dosis)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VacunacionDto>> ObtenerCampañasVacunacion()
        {
            try
            {
                using (HttpResponseMessage res = await _andesClient.GetAsync("modules/mobileApp/vacunas"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        return await res.Content.ReadFromJsonAsync<List<VacunacionDto>>();
                    }

                    string body = await res.Content.ReadAsStringAsync();
                    _logger.LogError("Obtener campañas de vacunación fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Se produjo un error al obtener las campañas de vacunación.");
            }
            return null;
        }
    }
}
