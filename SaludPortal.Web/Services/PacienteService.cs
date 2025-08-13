using AndesServices.Entities;

namespace SaludPortal.Web.Services
{
    public class PacienteService
    {
        private readonly IConfiguration _configuration;
        public PacienteService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Paciente> ObtenerPacientePorIdAsync(string token, string idPaciente)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return null;
            }
            try
            {
                AndesServices.Services.PacienteService miPacienteService = new AndesServices.Services.PacienteService(_configuration);
                return await miPacienteService.ObtenerPacientePorIdAsync(token, idPaciente);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener el paciente.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }

        public async Task<userLocation> ObtenerGeoreferenciaPacienteAsync(string direccion)
        {
            try
            {
                AndesServices.Services.PacienteService miPacienteService = new AndesServices.Services.PacienteService(_configuration);
                return await miPacienteService.ObtenerGeoreferenciaPaciente(direccion);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener la georeferencia.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }
    }
}
