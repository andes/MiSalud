using AndesServices.DTOs.Turnos;
using AndesServices.Entities;
using AndesServices.Interfaces;
using Newtonsoft.Json;

namespace AndesServices.Services
{
    public class MisTurnosService : IMisTurnos
    {
        private readonly ILogger<MisTurnosService> _logger;
        private readonly HttpClient _andesClient;

        public MisTurnosService(IHttpClientFactory httpClientFactory, ILogger<MisTurnosService> logger)
        {
            _logger = logger;
            _andesClient = httpClientFactory.CreateClient("Andes");
        }


        public Task<bool> ActualizarTurnoAsync(string idTurno, string motivoConsulta, string profesional, DateTime fechaHoraDacion)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CancelarTurnoAsync(string idTurno, string idBloque, string idAgenda, Paciente paciente)
        {
            try
            {

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

                using (HttpResponseMessage res = await _andesClient.SendAsync(request))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            string body = await res.Content.ReadAsStringAsync();
                            _logger.LogError("Cancelar turno fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                            return false;
                        }
                    }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al cancelar el turno");
            }
            return false;
        }

        public async Task<List<MisTurnos>> ObtenerMisTurnosAsync(string? documento = "")
        {
            try
            {
                using (HttpResponseMessage res = await _andesClient.GetAsync("modules/mobileApp/turnos"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        return await res.Content.ReadFromJsonAsync<List<MisTurnos>>();
                    }
                    else
                    {
                        string body = await res.Content.ReadAsStringAsync();
                        _logger.LogError("Obtener turnos fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                        return null;
                    }
                }                
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener los turnos");
                return null;
            }
        }

        public Task<MisTurnos> ObtenerTurnoPorIdAsync(string idTurno)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegistrarTurnoAsync(string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion, string tipoTurno)
        {
            try
            {
                var body = new RegistrarTurnoRequestDto
                {
                    IdAgenda = idAgenda,
                    IdBloque = idBloque,
                    IdTurno = idTurno,
                    Paciente = new RegistrarTurnoPacienteDto
                    {
                        Id = paciente.id,
                        Documento = paciente.documento,
                        Nombre = paciente.nombre,
                        Alias = paciente.alias,
                        Apellido = paciente.apellido,
                        FechaNacimiento = paciente.fechaNacimiento,
                        Telefono = paciente.telefono,
                        Sexo = paciente.sexo,
                        ObraSocial = paciente.obraSocial == null ? null : new RegistrarTurnoObraSocialDto
                        {
                            CodigoPuco = paciente.obraSocial.codigoPuco,
                            Nombre = paciente.obraSocial.nombre,
                            Financiador = paciente.obraSocial.financiador,
                            Origen = paciente.obraSocial.origen,
                            Prepaga = paciente.obraSocial.prepaga
                        }
                    },
                    TipoPrestacion = new RegistrarTurnoTipoPrestacionDto
                    {
                        Auditable = tipoPrestacion.auditable,
                        Ambito = tipoPrestacion.ambito,
                        Queries = tipoPrestacion.queries,
                        IdInterno = tipoPrestacion._id,
                        Fsn = tipoPrestacion.fsn,
                        SemanticTag = tipoPrestacion.semanticTag,
                        ConceptId = tipoPrestacion.conceptId,
                        Term = tipoPrestacion.term,
                        Multiprestacion = tipoPrestacion.multiprestacion,
                        Nombre = tipoPrestacion.nombre,
                        Id = tipoPrestacion.id
                    },
                    TipoTurno = tipoTurno,
                    EmitidoPor = "misalud",
                    Nota = "Solicitud realizada desde portal mi salud",
                    MotivoConsulta = ""
                };

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"modules/turnos/turno/{idTurno}/bloque/{idBloque}/agenda/{idAgenda}", UriKind.Relative),
                    Content = new StringContent(JsonConvert.SerializeObject(body), System.Text.Encoding.UTF8, "application/json")
                };

                using (HttpResponseMessage res = await _andesClient.SendAsync(request))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            string bodyResponse = await res.Content.ReadAsStringAsync();
                            _logger.LogError("Registrar turno fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, bodyResponse);
                            return false;
                        }
                    }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al registrar el turno");
            }
            return false;
        }

        public async Task<bool> RegistrarTurnoTeleConsultaAsync(string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion, string motivoConsulta, string telefono, string tipoTurno)
        {
            try
            {
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
                        tipoTurno,
                        emitidoPor = "misalud",
                        nota = "Cel.: " + telefono + ". Motivo: " + motivoConsulta,
                        estado = "solicitado",
                        motivoConsulta
                    }), System.Text.Encoding.UTF8, "application/json")
                };

                using (HttpResponseMessage res = await _andesClient.SendAsync(request))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            string body = await res.Content.ReadAsStringAsync();
                            _logger.LogError("Registrar turno fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                            return false;
                        }
                    }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al registrar el turno");
            }
            return false;
        }

        public async Task<List<OrganizacionAgenda>> ObtenerAgendasOrganizaciones(string idPaciente, userLocation userLocation, bool esTeleConsulta)
        {
            string estado = "disponible";

            try
            {
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

                using (HttpResponseMessage res = await _andesClient.SendAsync(request))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        List<OrganizacionAgenda?> organizacionAgendas = await res.Content.ReadFromJsonAsync<List<OrganizacionAgenda>>();
                        if (organizacionAgendas == null)
                        {
                            return null;
                        }

                        if (esTeleConsulta)
                        {
                            organizacionAgendas = await filtrarAgendasOrganizacionesTeleConsultaAsync(organizacionAgendas);
                        }

                        return organizacionAgendas;
                    }
                    else
                    {
                        string body = await res.Content.ReadAsStringAsync();
                        _logger.LogError("Obtener agendas fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                        return null;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener las agendas");
                return null;
            }
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

                using (HttpResponseMessage res = await _andesClient.SendAsync(request))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        List<ConceptoTurneable?> conceptosTurneables = await res.Content.ReadFromJsonAsync<List<ConceptoTurneable>>();
                        if (conceptosTurneables == null)
                        {
                            return null;
                        }

                        return conceptosTurneables;
                    }
                    else
                    {
                        string body = await res.Content.ReadAsStringAsync();
                        _logger.LogError("Obtener conceptos turneables fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                        return null;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener los conceptos turneables");
                return null;
            }
        }
    }
}
