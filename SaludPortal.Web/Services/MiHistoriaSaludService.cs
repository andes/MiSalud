using AndesServices.Entities;
using System.Net.Http;

namespace SaludPortal.Web.Services
{
    using CoreMiHistorialSaludService = AndesServices.Services.HistoriaSaludService;
    public class MiHistoriaSaludService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CoreMiHistorialSaludService> _coreLogger;
        public MiHistoriaSaludService(IConfiguration configuration
            , IHttpClientFactory httpClientFactory
            , ILogger<CoreMiHistorialSaludService> coreLogger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _coreLogger = coreLogger;
        }

        private CoreMiHistorialSaludService CreateCoreService()
            => new CoreMiHistorialSaludService(_configuration, _httpClientFactory, _coreLogger);
        public async Task<List<CategoriaHistoriaSalud>> ObtenerCategoriaHistoriaSaludAsync(string token)
        {
            try
            {
                return await CreateCoreService().ObtenerCategoriasHistoriaSaludAsync(token);
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
                return await CreateCoreService().ObtenerPrestacionesAsync(token, tipoPrestaciones, idPaciente, estado);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener las categorias.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }

        public async Task<Byte[]> DescargarCDAFilePorIdAsync(string token, string id)
        {
            try
            {
                return await CreateCoreService().DescargarCDAFilePorIdAsync(token, id);
            }
            catch (Exception)
            {

                throw;
            }

            return null;
        }
    }
}
