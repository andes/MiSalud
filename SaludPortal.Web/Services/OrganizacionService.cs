using AndesServices.Entities;
using AndesServices.Services;

namespace SaludPortal.Web.Services
{
    public class OrganizacionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public OrganizacionService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Organizacion> ObtenerOrganizacionPorIdAsync(string token, string id)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return null;
            }
            try
            {
                AndesServices.Services.OrganizacionService organizacionService = new AndesServices.Services.OrganizacionService(_httpClientFactory);
                return await organizacionService.ObtenerOrganizacionPorIdAsync(token, id);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener la organización.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }
    }
}
