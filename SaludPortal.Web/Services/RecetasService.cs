using AndesServices.Entities;
using AndesServices.Services;

namespace SaludPortal.Web.Services
{
    public class RecetasService
    {
        private readonly IConfiguration _configuration;

        public RecetasService(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<MisReceta>> ObtenerRecetasPacienteAsync(string token, string pacienteId)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return null;
            }
            try
            {
                MisRecetasService recetasService = new MisRecetasService(_configuration);
                return await recetasService.ObtenerRecetasPacienteAsync(token, pacienteId);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener las recetas del paciente.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }
    }
}
