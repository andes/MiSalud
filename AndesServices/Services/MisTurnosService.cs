using AndesServices.Entities;
using AndesServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace AndesServices.Services
{
    public class MisTurnosService : IMisTurnos
    {
        private IConfiguration? _configuration { get; }
        private readonly IHttpClientFactory _httpClientFactory;

        public MisTurnosService(IConfiguration? configuration, IHttpClientFactory? httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public Task<bool> ActualizarTurnoAsync(string token, string idTurno, string motivoConsulta, string profesional, DateTime fechaHoraDacion)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarTurnoAsync(string token, string idTurno)
        {
            throw new NotImplementedException();

        }

        public async Task<List<MisTurnos>> ObtenerMisTurnosAsync(string token, string? documento= "")
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/modules/mobileApp/turnos"
                : conexionServicios.UrlProyectoServiciosDemo + "/modules/mobileApp/turnos";

            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    using (HttpResponseMessage res = await client.GetAsync(url))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            List<MisTurnos?> misTurnos = await res.Content.ReadFromJsonAsync<List<MisTurnos>>();
                            if (misTurnos == null)
                            {
                                Console.WriteLine("No se encontraron turnos.");
                                return null;
                            }

                            return misTurnos;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener los turnos: {exception.Message}");
                return null;
            }
            return null;
        }

        public Task<MisTurnos> ObtenerTurnoPorIdAsync(string token, string idTurno)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegistrarTurnoAsync(string token, string documento, string motivoConsulta, string profesional, string tipoPrestacion, DateTime fechaHoraDacion, string organizacionId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrganizacionAgenda>> ObtenerAgendasOrganizaciones(string token, string idPaciente, userLocation userLocation)
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/modules/mobileApp/agendasDisponibles"
                : conexionServicios.UrlProyectoServiciosDemo + "/modules/mobileApp/agendasDisponibles";

            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(url),
                        Content = new StringContent(JsonConvert.SerializeObject(new { idPaciente, userLocation }), System.Text.Encoding.UTF8, "application/json")
                    };

                    
                    using (HttpResponseMessage res = await client.SendAsync(request))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            List<OrganizacionAgenda?> organizacionAgendas = await res.Content.ReadFromJsonAsync<List<OrganizacionAgenda>>();
                            if (organizacionAgendas == null)
                            {
                                Console.WriteLine("No se encontraron agendas.");
                                return null;
                            }

                            return organizacionAgendas;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener las agendas: {exception.Message}");
                return null;
            }
            return null;
        }
    }
}
