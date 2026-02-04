using AndesServices.Entities;

namespace SaludPortal.Web.Services
{
    public class VacunacionService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VacunacionService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Vacunacion>> ObtenerCampañasVacunacion(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return null;
            }

            try
            {
                AndesServices.Services.VacunacionService vacunacionService = new AndesServices.Services.VacunacionService(_httpClientFactory);
                return await vacunacionService.ObtenerCampañasVacunacion(token);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener las campañas de vacunación.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }
    }
}
