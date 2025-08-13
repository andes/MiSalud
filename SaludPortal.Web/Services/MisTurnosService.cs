using AndesServices.Entities;

namespace SaludPortal.Web.Services
{
    public class MisTurnosService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public MisTurnosService(IConfiguration? configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<MisTurnos>> ObtenerMisTurnosAsync(string token, string? documento = "")
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return null;
            }
            try
            {
                AndesServices.Services.MisTurnosService misTurnosService = new AndesServices.Services.MisTurnosService(_configuration, _httpClientFactory);
                return await misTurnosService.ObtenerMisTurnosAsync(token, documento);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener los turnos.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }

        public async Task<List<OrganizacionAgenda>> ObtenerAgendasOrganizaciones(string token, string idPaciente, userLocation userLocation)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return null;
            }
            try
            {
                AndesServices.Services.MisTurnosService misTurnosService = new AndesServices.Services.MisTurnosService(_configuration, _httpClientFactory);
                return await misTurnosService.ObtenerAgendasOrganizaciones(token, idPaciente, userLocation);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener las agendas.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }
    }
}
