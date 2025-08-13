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
    }
}
