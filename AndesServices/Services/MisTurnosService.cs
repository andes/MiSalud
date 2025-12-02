using AndesServices.Entities;
using AndesServices.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace AndesServices.Services
{
    public class MisTurnosService : IMisTurnos
    {
        private IConfiguration _configuration { get; }
        private readonly IHttpClientFactory _httpClientFactory;

        public MisTurnosService(IConfiguration? configuration, IHttpClientFactory? httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        private string GetServiciosBaseUrl()
        {
            var cfg = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(cfg);
            var baseUrl = cfg.usarProd ? cfg.UrlProyectoServiciosProd : cfg.UrlProyectoServiciosDemo;
            return baseUrl.TrimEnd('/');
        }

        public Task<bool> ActualizarTurnoAsync(string token, string idTurno, string motivoConsulta, string profesional, DateTime fechaHoraDacion)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CancelarTurnoAsync(string token, string idTurno, string idBloque, string idAgenda, Paciente paciente)
        {
            string url = GetServiciosBaseUrl() + "/modules/mobileApp/turnos/cancelar";

            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    string jsonCancelarTurno = $@"{{
                        ""agenda_id"": ""{idAgenda}"",
                        ""bloque_id"": ""{idBloque}"",
                        ""turno_id"": ""{idTurno}"",
                        ""familiar"": {{
                            ""id"": ""{paciente.id}"",
    	                    ""documento"": ""{paciente.documento}"",
    	                    ""apellido"": ""{paciente.apellido}"",
    	                    ""nombre"": ""{paciente.nombre}"",
    	                    ""alias"": ""{paciente.alias}"",
    	                    ""fechaNacimiento"": ""{paciente.fechaNacimiento}"",
    	                    ""sexo"": ""{paciente.sexo}"",
    	                    ""telefono"": ""{paciente.telefono}""}}
                    }}";

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(url),
                        Content = new StringContent(jsonCancelarTurno, System.Text.Encoding.UTF8, "application/json")
                    };

                    using (HttpResponseMessage res = await client.SendAsync(request))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Turno cancelado.");
                            return true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al cancelar el turno: {exception.Message}");
            }
            return false;
        }

        public async Task<List<MisTurnos>> ObtenerMisTurnosAsync(string token, string? documento = "")
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = GetServiciosBaseUrl() + "/modules/mobileApp/turnos";

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

        public async Task<bool> RegistrarTurnoAsync(string token, string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion)
        {
            string url = GetServiciosBaseUrl() + "/modules/turnos";

            url += $"/turno/{idTurno}/bloque/{idBloque}/agenda/{idAgenda}";
            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    string tipoTurno = "programado";
                    string emitidoPor = "misalud";
                    string nota = "Turno pedido desde portal mi salud";
                    string motivoConsulta = "";

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Patch,
                        RequestUri = new Uri(url),
                        Content = new StringContent(JsonConvert.SerializeObject(new
                        {
                            idAgenda,
                            idBloque,
                            idTurno,
                            paciente,
                            tipoPrestacion,
                            tipoTurno,
                            emitidoPor,
                            nota,
                            motivoConsulta
                        }), System.Text.Encoding.UTF8, "application/json")
                    };

                    using (HttpResponseMessage res = await client.SendAsync(request))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Turno confirmado.");
                            return true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al registrar el turno: {exception.Message}");
            }
            return false; // Ensure a boolean is returned in case of failure
        }

        public async Task<List<OrganizacionAgenda>> ObtenerAgendasOrganizaciones(string token, string idPaciente, userLocation userLocation)
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/modules/mobileApp/agendasDisponibles"
                : conexionServicios.UrlProyectoServiciosDemo + "/modules/mobileApp/agendasDisponibles";
            string estado = "disponible";

            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    var userLocationJson = JsonConvert.SerializeObject(userLocation);
                    var queryParams = new Dictionary<string, string?>
                    {
                        ["estado"] = estado,
                        ["userLocation"] = userLocationJson
                    };

                    string finalUrl = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(url, queryParams);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(finalUrl)
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
