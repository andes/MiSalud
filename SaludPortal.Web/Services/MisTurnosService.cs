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

        public async Task<List<OrganizacionAgenda>> ObtenerAgendasOrganizaciones(string token, string idPaciente, userLocation userLocation, bool esTeleconsulta)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return null;
            }
            try
            {
                AndesServices.Services.MisTurnosService misTurnosService = new AndesServices.Services.MisTurnosService(_configuration, _httpClientFactory);
                return await misTurnosService.ObtenerAgendasOrganizaciones(token, idPaciente, userLocation, esTeleconsulta);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener las agendas.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }
        public async Task<bool> RegistrarTurnoAsync(string token, string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return false;
            }
            try
            {
                AndesServices.Services.MisTurnosService misTurnosService = new AndesServices.Services.MisTurnosService(_configuration, _httpClientFactory);
                return await misTurnosService.RegistrarTurnoAsync(token, idTurno, idBloque, idAgenda, paciente, tipoPrestacion);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al registrar el turno.");
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        public async Task<bool> RegistrarTurnoTeleConsultaAsync(string token, string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion, string motivoConsulta = "", string estado ="")
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return false;
            }
            try
            {
                AndesServices.Services.MisTurnosService misTurnosService = new AndesServices.Services.MisTurnosService(_configuration, _httpClientFactory);
                return await misTurnosService.RegistrarTurnoTeleConsultaAsync(token, idTurno, idBloque, idAgenda, paciente, tipoPrestacion, motivoConsulta, estado);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al registrar el turno.");
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        public async Task<bool> CancelarTurnoAsync(string token, string idTurno, string idBloque, string idAgenda, Paciente paciente)
        {
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token no proporcionado.");
                return false;
            }
            try
            {
                AndesServices.Services.MisTurnosService misTurnosService = new AndesServices.Services.MisTurnosService(_configuration, _httpClientFactory);
                return await misTurnosService.CancelarTurnoAsync(token, idTurno, idBloque, idAgenda, paciente);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al cancelar el turno.");
                Console.WriteLine(exception.Message);
                return false;
            }
        }
    }
}
