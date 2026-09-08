using AndesServices.Entities;
using AndesServices.Interfaces;

namespace AndesServices.Services
{
    public class FarmaciasTurnoService : IFarmaciasTurno
    {
        private readonly HttpClient _andesClient;
        private readonly ILogger<FarmaciasTurnoService> _logger;

        public FarmaciasTurnoService(IHttpClientFactory httpClientFactory, ILogger<FarmaciasTurnoService> logger)
        {
            _andesClient = httpClientFactory.CreateClient("Andes-NoJWT");
            _logger = logger;
        }

        public async Task<List<FarmaciasTurno>?> ObtenerFarmaciasTurnoAsync(string localidadId, string fechaDesde, string fechaHasta)
        {
            try
            {
                string url = $"modules/mobileApp/farmacias/turnos?localidad={localidadId}&desde={fechaDesde}&hasta={fechaHasta}";

                using (HttpResponseMessage res = await _andesClient.GetAsync(url))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        return await res.Content.ReadFromJsonAsync<List<FarmaciasTurno>>();
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error al obtener las farmacias de turno para la localidad {localidadId}.");
                return null;
            }
            return null;
        }

        public async Task<List<Localidad>?> ObtenerLocalidadesAsync()
        {
            try
            {
                using (HttpResponseMessage res = await _andesClient.GetAsync("modules/mobileApp/farmacias/localidades"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        List<Localidad>? LstLocalidades = await res.Content.ReadFromJsonAsync<List<Localidad>>();
                        if (LstLocalidades == null)
                        {
                            return null;
                        }

                        return LstLocalidades;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener las localidades de farmacias.");
                return null;
            }
            return null;
        }
    }
}
