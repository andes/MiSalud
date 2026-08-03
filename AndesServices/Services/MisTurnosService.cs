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
        private readonly IHttpClientFactory _httpClientFactory;

        public MisTurnosService(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public Task<bool> ActualizarTurnoAsync(string idTurno, string motivoConsulta, string profesional, DateTime fechaHoraDacion)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CancelarTurnoAsync(string idTurno, string idBloque, string idAgenda, Paciente paciente)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");

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
                    RequestUri = new Uri("modules/mobileApp/turnos/cancelar", UriKind.Relative),
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
            catch (Exception exception)
            {
                Console.WriteLine($"Error al cancelar el turno: {exception.Message}");
            }
            return false;
        }

        public async Task<List<MisTurnos>> ObtenerMisTurnosAsync(string? documento = "")
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");

                using (HttpResponseMessage res = await client.GetAsync("modules/mobileApp/turnos"))
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
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener los turnos: {exception.Message}");
                return null;
            }
            return null;
        }

        public Task<MisTurnos> ObtenerTurnoPorIdAsync(string idTurno)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegistrarTurnoAsync(string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"modules/turnos/turno/{idTurno}/bloque/{idBloque}/agenda/{idAgenda}", UriKind.Relative),
                    Content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        idAgenda,
                        idBloque,
                        idTurno,
                        paciente,
                        tipoPrestacion,
                        tipoTurno = "mobile",
                        emitidoPor = "misalud",
                        nota = "Solicitud realizada desde portal mi salud"
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
            catch (Exception exception)
            {
                Console.WriteLine($"Error al registrar el turno: {exception.Message}");
            }
            return false;
        }

        public async Task<bool> RegistrarTurnoTeleConsultaAsync(string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion, string motivoConsulta, string telefono)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"modules/turnos/turno/{idTurno}/bloque/{idBloque}/agenda/{idAgenda}", UriKind.Relative),
                    Content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        idAgenda,
                        idBloque,
                        idTurno,
                        paciente,
                        tipoPrestacion,
                        tipoTurno = "programado",
                        emitidoPor = "misalud",
                        nota = "Cel.: " + telefono + ". Motivo: " + motivoConsulta,
                        estado = "solicitado",
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
            catch (Exception exception)
            {
                Console.WriteLine($"Error al registrar el turno: {exception.Message}");
            }
            return false;
        }

        public async Task<List<OrganizacionAgenda>> ObtenerAgendasOrganizaciones(string idPaciente, userLocation userLocation, bool esTeleConsulta)
        {
            string estado = "disponible";

            try
            {
                var client = _httpClientFactory.CreateClient("Andes");

                var userLocationJson = JsonConvert.SerializeObject(userLocation);
                var queryParams = new Dictionary<string, string?>
                {
                    ["idPaciente"] = idPaciente,
                    ["estado"] = estado,
                    ["userLocation"] = userLocationJson,
                    ["teleConsulta"] = esTeleConsulta.ToString().ToLower(),
                };

                string finalUrl = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString("modules/mobileApp/agendasDisponibles", queryParams);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(finalUrl, UriKind.Relative)
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

                        if (esTeleConsulta)
                        {
                            organizacionAgendas = await filtrarAgendasOrganizacionesTeleConsultaAsync(organizacionAgendas);
                        }

                        return organizacionAgendas;
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

        private async Task<List<OrganizacionAgenda>> filtrarAgendasOrganizacionesTeleConsultaAsync(List<OrganizacionAgenda> organizacionAgendas)
        {
            List<ConceptoTurneable> conceptosTurneables = await ObtenerConceptosTurneablesAsync(true);

            if (conceptosTurneables != null && conceptosTurneables.Count > 0)
            {
                var conceptIdsValidos = new HashSet<string>(conceptosTurneables.Count);
                for (int i = 0; i < conceptosTurneables.Count; i++)
                {
                    conceptIdsValidos.Add(conceptosTurneables[i].conceptId);
                }

                // Filtrar in-place organizaciones → agendas → bloques
                for (int i = organizacionAgendas.Count - 1; i >= 0; i--)
                {
                    var org = organizacionAgendas[i];
                    if (org?.agendas == null)
                    {
                        organizacionAgendas.RemoveAt(i);
                        continue;
                    }

                    for (int j = org.agendas.Count - 1; j >= 0; j--)
                    {
                        var agenda = org.agendas[j];
                        if (agenda?.bloques == null)
                        {
                            org.agendas.RemoveAt(j);
                            continue;
                        }

                        for (int k = agenda.bloques.Count - 1; k >= 0; k--)
                        {
                            var bloque = agenda.bloques[k];
                            if (bloque?.tipoPrestaciones == null)
                            {
                                agenda.bloques.RemoveAt(k);
                                continue;
                            }

                            bool tieneConceptoValido = false;
                            for (int l = 0; l < bloque.tipoPrestaciones.Count; l++)
                            {
                                var conceptId = bloque.tipoPrestaciones[l]?.conceptId;
                                if (conceptId != null && conceptIdsValidos.Contains(conceptId))
                                {
                                    tieneConceptoValido = true;
                                    break;
                                }
                            }

                            if (!tieneConceptoValido)
                            {
                                agenda.bloques.RemoveAt(k);
                            }
                        }

                        if (agenda.bloques.Count == 0)
                        {
                            org.agendas.RemoveAt(j);
                        }
                    }

                    if (org.agendas.Count == 0)
                    {
                        organizacionAgendas.RemoveAt(i);
                    }
                }
            }

            return organizacionAgendas;
        }

        public async Task<List<ConceptoTurneable>> ObtenerConceptosTurneablesAsync(bool esTeleConsulta = false)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");

                var queryParams = new Dictionary<string, string?>
                {
                    ["teleConsulta"] = esTeleConsulta.ToString().ToLower(),
                };

                string finalUrl = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString("core/tm/conceptos-turneables", queryParams);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(finalUrl, UriKind.Relative)
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
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener los conceptos turneables: {exception.Message}");
                return null;
            }
            return null;
        }
    }
}
