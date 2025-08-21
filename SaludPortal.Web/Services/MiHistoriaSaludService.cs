using AndesServices.Entities;

namespace SaludPortal.Web.Services
{
    public class MiHistoriaSaludService
    {
        private readonly IConfiguration _configuration;
        public MiHistoriaSaludService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<CategoriaHistoriaSalud>> ObtenerCategoriaHistoriaSaludAsync(string token)
        {
            try
            {
                AndesServices.Services.HistoriaSaludService categoriasHistoriaSalud = new AndesServices.Services.HistoriaSaludService(_configuration);
                return await categoriasHistoriaSalud.ObtenerCategoriasHistoriaSaludAsync(token);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener las categorias.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }

        public async Task<List<PrestacionHistoriaSalud>> ObtenerPrestacionesAsync(string token, string tipoPrestaciones, string idPaciente, string estado = "validada")
        {
            try
            {
                AndesServices.Services.HistoriaSaludService prestacionesHistoriaSalud = new AndesServices.Services.HistoriaSaludService(_configuration);
                return await prestacionesHistoriaSalud.ObtenerPrestacionesAsync(token, tipoPrestaciones, idPaciente, estado);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener las categorias.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }
    }
}
