using AndesServices.Interfaces;
using SaludPortal.Application.DTOs.Vacunaciones;

namespace AndesServices.Services
{
    public class VacunacionService : IVacunacion
    {
        private readonly HttpClient _httpClient;

        public VacunacionService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Andes");
        }

        public async Task<List<VacunacionDto>> ObtenerCampañasVacunacion()
        {
            try
            {
                using (HttpResponseMessage res = await _httpClient.GetAsync("modules/mobileApp/vacunas"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        List<VacunacionDto?> vacunacion = await res.Content.ReadFromJsonAsync<List<VacunacionDto>>();
                        if (vacunacion == null)
                        {
                            Console.WriteLine("No se encontraron campañas de vacunación.");
                            return null;
                        }

                        return vacunacion;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error");
                Console.WriteLine(exception.Message);
            }
            return null;
        }
    }
}
