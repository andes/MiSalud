using AndesServices.Entities;

namespace SaludPortal.Web.Services
{
    public class MisLaboratoriosService
    {
        private readonly IConfiguration _configuration;
        public MisLaboratoriosService(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<MisLaboratorios>> ObtenerMisLaboratoriosAsync(string token, string pacienteId, string fechaDde, string fechaHta)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return null;
            }
            try
            {
                AndesServices.Services.MisLaboratoriosService misLaboratoriosService = new AndesServices.Services.MisLaboratoriosService(_configuration);
                return await misLaboratoriosService.ObtenerMisLaboratoriosAsync(token, pacienteId, fechaDde, fechaHta);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener los laboratorios.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }

        public async Task<byte[]> descargarLaboratorio(string token, string idProtocolo, string documento)
        {
            byte[] unByte = null;
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return await Task.FromResult(unByte);
            }
            try
            {
                AndesServices.Services.MisLaboratoriosService misLaboratoriosService = new AndesServices.Services.MisLaboratoriosService(_configuration);
                return await misLaboratoriosService.DescargarLaboratorioPorIdAsync(token, idProtocolo, documento);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener el archivo.");
                Console.WriteLine(exception.Message);
            }
            return await Task.FromResult(unByte);
        }
    }
}