using AndesServices.Entities;

namespace SaludPortal.Web.Services
{
    public class VacunacionService
    {
        private readonly IConfiguration _configuration;

        public VacunacionService(IConfiguration? configuration)
        {
            _configuration = configuration;
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
                AndesServices.Services.VacunacionService vacunacionService = new AndesServices.Services.VacunacionService(_configuration);
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
