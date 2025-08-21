using AndesServices.Entities;
using AndesServices.Services;
using Newtonsoft.Json.Linq;

namespace SaludPortal.Web.Services
{
    using CoreMisLabsService = AndesServices.Services.MisLaboratoriosService;
    public class MisLaboratoriosService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CoreMisLabsService> _coreLogger;

        public MisLaboratoriosService(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<CoreMisLabsService> coreLogger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _coreLogger = coreLogger;
        }

        private CoreMisLabsService CreateCoreService()
            => new CoreMisLabsService(_configuration, _httpClientFactory, _coreLogger);

        public async Task<List<MisLaboratorios>> ObtenerMisLaboratoriosAsync(string token, string pacienteId, string fechaDde, string fechaHta)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                _coreLogger.LogWarning("Token no proporcionado.");
                return null;
            }

            try
            {
                return await CreateCoreService()
                    .ObtenerMisLaboratoriosAsync(token, pacienteId, fechaDde, fechaHta);
            }
            catch (Exception ex)
            {
                _coreLogger.LogError(ex, "Error obteniendo laboratorios paciente {PacienteId}", pacienteId);
                return null;
            }
        }
        public async Task<byte[]> DescargarLaboratorio(string token, string idProtocolo, string documento)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                _coreLogger.LogWarning("Token no proporcionado (descarga).");
                return null;
            }

            try
            {
                return await CreateCoreService()
                    .DescargarLaboratorioPorIdAsync(token, idProtocolo, documento);
            }
            catch (Exception ex)
            {
                _coreLogger.LogError(ex, "Error descargando laboratorio {IdProtocolo}", idProtocolo);
                return null;
            }
        }

        public async Task<List<LaboratoriosLachybs>> ObtenerMisLaboratoriosLACHYBSAsync(string usuario, string clave, string documento)
        {
            try
            {
                return await CreateCoreService()
                    .ObtenerMisLaboratoriosLACHYBSAsync(usuario, clave, documento);
            }
            catch (Exception ex)
            {
                _coreLogger.LogError(ex, "Error obteniendo LACHYBS dni {Documento}", documento);
                return null;
            }
        }
        public async Task<string> DescargarLaboratorioLACHyBSPorIdAsync(string usuario, string clave,string idProtocolo)
        {
            try
            {
                return await CreateCoreService()
                    .DescargarLaboratorioLACHyBSPorIdAsync(usuario, clave, idProtocolo);
            }
            catch (Exception ex)
            {
                _coreLogger.LogError(ex, "Error obteniendo LACHYBS dni {idProtocolo}", idProtocolo);
                return null;
            }
        }
    }
}