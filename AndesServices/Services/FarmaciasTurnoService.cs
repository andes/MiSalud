using AndesServices.Entities;
using AndesServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace AndesServices.Services
{
    public class FarmaciasTurnoService : IFarmaciasTurno
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FarmaciasTurnoService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<FarmaciasTurno>> ObtenerFarmaciasTurnoAsync(string localidadId, string fechaDesde, string fechaHasta)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes-NoJWT");
                string url = $"modules/mobileApp/farmacias/turnos?localidad={localidadId}&desde={fechaDesde}&hasta={fechaHasta}";

                using (HttpResponseMessage res = await client.GetAsync(url))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        List<FarmaciasTurno>? LstFarmacias = await res.Content.ReadFromJsonAsync<List<FarmaciasTurno>>();
                        if (LstFarmacias == null)
                        {
                            Console.WriteLine("No se encontraron farmacias disponibles.");
                            return null;
                        }

                        return LstFarmacias;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener las farmacias: {exception.Message}");
                return null;
            }
            return null;
        }

        public async Task<List<Localidad>> ObtenerLocalidadesAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes-NoJWT");
                using (HttpResponseMessage res = await client.GetAsync("modules/mobileApp/farmacias/localidades"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        List<Localidad>? LstLocalidades = await res.Content.ReadFromJsonAsync<List<Localidad>>();
                        if (LstLocalidades == null)
                        {
                            Console.WriteLine("No se encontraron farmacias disponibles.");
                            return null;
                        }

                        return LstLocalidades;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener las localidades: {exception.Message}");
                return null;
            }
            return null;
        }
    }
}
