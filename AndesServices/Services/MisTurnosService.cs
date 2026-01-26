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
                    string nota = "Solicitud realizada desde portal mi salud";

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
                            nota
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
            return false;
        }

        public async Task<bool> RegistrarTurnoTeleConsultaAsync(string token, string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion, string motivoConsulta = "", string estado = "")
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
                    string nota = "Cel.: " + paciente.telefono + ". Motivo: " + motivoConsulta;

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
                            estado,
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
            return false;
        }

        public async Task<List<OrganizacionAgenda>> ObtenerAgendasOrganizaciones(string token, string idPaciente, userLocation userLocation, bool esTeleConsulta)
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

                            organizacionAgendas = await filtrarAgendasOrganizacionesTeleConsultaAsync(organizacionAgendas, esTeleConsulta);

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

        private async Task<List<OrganizacionAgenda>> filtrarAgendasOrganizacionesTeleConsultaAsync(List<OrganizacionAgenda> organizacionAgendas, bool esTeleConsulta = false)
        {
            // TEMPORAL HASTA TANTO SE CORRIJA EL ENDPOINT
            // -.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-
            const string conceptIdTeleconsulta = "6421000013106";
            // Filtrar in-place sin crear nuevos objetos
            for (int i = organizacionAgendas.Count - 1; i >= 0; i--)
            {
                var org = organizacionAgendas[i];
                if (org?.agendas == null)
                {
                    organizacionAgendas.RemoveAt(i);
                    continue;
                }

                // Filtrar agendas
                for (int j = org.agendas.Count - 1; j >= 0; j--)
                {
                    var agenda = org.agendas[j];
                    if (agenda?.bloques == null)
                    {
                        org.agendas.RemoveAt(j);
                        continue;
                    }

                    // Filtrar bloques según el criterio
                    for (int k = agenda.bloques.Count - 1; k >= 0; k--)
                    {
                        var bloque = agenda.bloques[k];
                        if (bloque?.tipoPrestaciones == null)
                        {
                            agenda.bloques.RemoveAt(k);
                            continue;
                        }

                        bool cumpleCriterio = false;
                        if (esTeleConsulta)
                        {
                            // Buscar si contiene el conceptId de teleconsulta
                            for (int l = 0; l < bloque.tipoPrestaciones.Count; l++)
                            {
                                if (bloque.tipoPrestaciones[l]?.conceptId?.Contains(conceptIdTeleconsulta) == true)
                                {
                                    cumpleCriterio = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // Buscar si NO contiene el conceptId de teleconsulta
                            for (int l = 0; l < bloque.tipoPrestaciones.Count; l++)
                            {
                                if (bloque.tipoPrestaciones[l]?.conceptId != conceptIdTeleconsulta)
                                {
                                    cumpleCriterio = true;
                                    break;
                                }
                            }
                        }

                        if (!cumpleCriterio)
                        {
                            agenda.bloques.RemoveAt(k);
                        }
                    }

                    // Remover agenda si no tiene bloques válidos
                    if (agenda.bloques.Count == 0)
                    {
                        org.agendas.RemoveAt(j);
                    }
                }

                // Remover organización si no tiene agendas válidas
                if (org.agendas.Count == 0)
                {
                    organizacionAgendas.RemoveAt(i);
                }
            }


            //List<ConceptoTurneable> conceptosTurneables = await ObtenerConceptosTurneablesAsync(token, esTeleconsulta);

            //if (conceptosTurneables != null && conceptosTurneables.Count > 0)
            //{
            //    var conceptIdsValidos = new HashSet<string>(conceptosTurneables.Count);
            //    for (int i = 0; i < conceptosTurneables.Count; i++)
            //    {
            //        conceptIdsValidos.Add(conceptosTurneables[i].conceptId);
            //    }

            //    // Filtrar in-place organizaciones → agendas → bloques
            //    for (int i = organizacionAgendas.Count - 1; i >= 0; i--)
            //    {
            //        var org = organizacionAgendas[i];
            //        if (org?.agendas == null)
            //        {
            //            organizacionAgendas.RemoveAt(i);
            //            continue;
            //        }

            //        for (int j = org.agendas.Count - 1; j >= 0; j--)
            //        {
            //            var agenda = org.agendas[j];
            //            if (agenda?.bloques == null)
            //            {
            //                org.agendas.RemoveAt(j);
            //                continue;
            //            }

            //            for (int k = agenda.bloques.Count - 1; k >= 0; k--)
            //            {
            //                var bloque = agenda.bloques[k];
            //                if (bloque?.tipoPrestaciones == null)
            //                {
            //                    agenda.bloques.RemoveAt(k);
            //                    continue;
            //                }

            //                bool tieneConceptoValido = false;
            //                for (int l = 0; l < bloque.tipoPrestaciones.Count; l++)
            //                {
            //                    var conceptId = bloque.tipoPrestaciones[l]?.conceptId;
            //                    if (conceptId != null && conceptIdsValidos.Contains(conceptId))
            //                    {
            //                        tieneConceptoValido = true;
            //                        break;
            //                    }
            //                }

            //                if (!tieneConceptoValido)
            //                {
            //                    agenda.bloques.RemoveAt(k);
            //                }
            //            }

            //            if (agenda.bloques.Count == 0)
            //            {
            //                org.agendas.RemoveAt(j);
            //            }
            //        }

            //        if (org.agendas.Count == 0)
            //        {
            //            organizacionAgendas.RemoveAt(i);
            //        }
            //    }
            //}

            return organizacionAgendas;
        }

        public async Task<List<ConceptoTurneable>> ObtenerConceptosTurneablesAsync(string token, bool esTeleConsulta = false)
        {
            string url = GetServiciosBaseUrl() + "/core/tm/conceptos-turneables";

            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    var queryParams = new Dictionary<string, string?>
                    {
                        ["teleConsulta"] = esTeleConsulta.ToString().ToLower(),
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
                            List<ConceptoTurneable?> conceptosTurneables = await res.Content.ReadFromJsonAsync<List<ConceptoTurneable>>();
                            if (conceptosTurneables == null)
                            {
                                Console.WriteLine("No se encontraron conceptos turneables.");
                                return null;
                            }

                            return conceptosTurneables;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener los conceptos turneables: {exception.Message}");
                return null;
            }
            return null;
        }
    }
}
