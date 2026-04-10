using AndesServices.DTOs.Prestaciones;
using AndesServices.Entities;
using AndesServices.Interfaces;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace AndesServices.Services
{
    public class HistoriaSaludService : IHistoriaSalud
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HistoriaSaludService> _logger;

        public HistoriaSaludService(
            IHttpClientFactory httpClientFactory,
            ILogger<HistoriaSaludService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // Implementación de los métodos de la interfaz IHistoriaSalud
        public async Task<List<CategoriaHistoriaSalud>> ObtenerCategoriasHistoriaSaludAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");

                using (HttpResponseMessage res = await client.GetAsync("modules/mobileApp/categoria"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        List<CategoriaHistoriaSalud?> categoriaHistoriaSalud = new List<CategoriaHistoriaSalud?>();

                        List<CategoriaHistoriaSalud>? listaCategoriasHistoriaSalud = await res.Content.ReadFromJsonAsync<List<CategoriaHistoriaSalud>>();

                        if (listaCategoriasHistoriaSalud == null)
                        {
                            Console.WriteLine("No se encontraron categorias.");
                            return null;
                        }

                        return listaCategoriasHistoriaSalud;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener las categorias: {exception.Message}");
                return null;
            }
            return null;
        }

        public async Task<List<PrestacionHistoriaSalud>> ObtenerPrestacionesAsync(string tipoPrestaciones, string idPaciente, string estado = "validada")
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");
                
                bool esCda = (tipoPrestaciones == "90226004" || tipoPrestaciones == "86273004");
                string url;
                
                if (esCda)
                {
                    url = $"modules/cda/paciente/{idPaciente}";
                }
                else
                {
                    url = $"modules/rup/prestaciones?tipoPrestaciones={tipoPrestaciones}&idPaciente={idPaciente}&estado={estado}";
                }

                using (HttpResponseMessage res = await client.GetAsync(url))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        try
                        {
                            if (esCda)
                            {
                                // Deserializa CDA y mapea a PrestacionHistoriaSalud
                                var cdaDocs = await res.Content.ReadFromJsonAsync<List<CdaDocumento>>();
                                if (cdaDocs == null || cdaDocs.Count == 0)
                                {
                                    Console.WriteLine("No se encontraron documentos CDA.");
                                    return null;
                                }

                                var mapped = new List<PrestacionHistoriaSalud>(cdaDocs.Count);
                                foreach (var d in cdaDocs.Where(c => c.prestacion?.snomed?.conceptId == tipoPrestaciones))
                                {
                                    mapped.Add(MapCdaToPrestacion(d));
                                }
                                return mapped;
                            }
                            else
                            {
                                // RUP
                                List<PrestacionHistoriaSalud>? listaPrestacionesHistoriaSalud = await res.Content.ReadFromJsonAsync<List<PrestacionHistoriaSalud>>();

                                if (listaPrestacionesHistoriaSalud == null)
                                {
                                    Console.WriteLine("No se encontraron prestaciones.");
                                    return null;
                                }

                                return listaPrestacionesHistoriaSalud;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener las prestaciones: {exception.Message}");
                return null;
            }
            return null;
        }

        public async Task<Byte[]?> DescargarCDAFilePorIdAsync(string id)
        {
            byte[] unByte = null;

            try
            {
                var client = _httpClientFactory.CreateClient("Andes");
                //var parametrosBody = new StringContent("{\"protocolo\":{\"data\":{\"idProtocolo\":" + idProtocolo + ",\"documento\":" + documento + "}}}", System.Text.Encoding.UTF8, "application/json");
                using (HttpResponseMessage res = await client.GetAsync($"modules/cda/{id}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        byte[]? fileResponse = await res.Content.ReadAsByteArrayAsync();
                        if (fileResponse == null)
                        {
                            Console.WriteLine("Error: File is null.");
                            return await Task.FromResult(unByte);
                        }

                        return fileResponse;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener el archivo del CDA: {exception.Message}");
                return await Task.FromResult(unByte);
            }
            return await Task.FromResult(unByte);
        }

        private static PrestacionHistoriaSalud MapCdaToPrestacion(CdaDocumento d)
        {
            var fecha = d.fecha?.DateTime;

            // Construye el concepto SNOMED desde el CDA
            ConceptoExtendido? concepto = null;
            if (d.prestacion?.snomed != null)
            {
                concepto = new ConceptoExtendido
                {
                    conceptId = d.prestacion.snomed.conceptId,
                    term = d.prestacion.snomed.term,
                    fsn = d.prestacion.snomed.fsn,
                    semanticTag = d.prestacion.snomed.semanticTag
                };
            }

            // Arma un paciente mínimo con el identificador del CDA (evita nullables obligatorios)
            Paciente? paciente = null;
            if (!string.IsNullOrWhiteSpace(d.paciente))
            {
                paciente = new Paciente
                {
                    id = d.paciente,
                    _id = d.paciente,
                    adjuntos = d.adjuntos
                };
            }

            return new PrestacionHistoriaSalud
            {
                _id = d.cda_id,
                id = d.cda_id,
                paciente = paciente,
                createdAt = d.fecha,
                updatedAt = d.fecha,
                solicitud = new SolicitudPrestacion
                {
                    tipoPrestacion = concepto,
                    profesional = d.profesional,
                    organizacion = d.organizacion,
                    fecha = fecha
                },
                ejecucion = new EjecucionPrestacion
                {
                    organizacion = d.organizacion,
                    fecha = fecha
                }
            };
        }

        public async Task<Byte[]?> DescargarPdfPrestacionAsync(string idPrestacion)
        {
            var dto = new DescargarArchivoPrestacionDto
            {
                IdPrestacion = idPrestacion
            };

            byte[] bytes = null;

            try
            {
                var client = _httpClientFactory.CreateClient("Andes");
                using (HttpResponseMessage res = await client.PostAsJsonAsync($"modules/descargas", dto))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        byte[]? fileResponse = await res.Content.ReadAsByteArrayAsync();
                        if (fileResponse == null)
                        {
                            Console.WriteLine("Error: File is null.");
                            return bytes;
                        }

                        return fileResponse;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener el archivo de la prestación: {exception.Message}");
                return bytes;
            }
            return bytes;
        }

        public async Task<string?> ObtenerFileTokenAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");
                using (HttpResponseMessage res = await client.PostAsync("auth/file-token", null))
                {
                    if (!res.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("No se pudo obtener el file-token. Status: {Status}", res.StatusCode);
                        return null;
                    }

                    var tokenResponse = await res.Content.ReadFromJsonAsync<FileTokenResponseDto>();
                    if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.Token))
                    {
                        _logger.LogWarning("El file-token recibido está vacío.");
                        return null;
                    }

                    return tokenResponse.Token;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener el file-token.");
                return null;
            }
        }

        public async Task<string?> ObtenerImagenPrestacionUrlAsync(string idPrestacion, string fileToken)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");
                var baseAddress = client.BaseAddress?.ToString().TrimEnd('/');
                var pacsUrl = $"{baseAddress}/modules/rup/prestaciones/{idPrestacion}/pacs?token={fileToken}";

                using var checkRes = await client.GetAsync(pacsUrl, HttpCompletionOption.ResponseHeadersRead);
                if (!checkRes.IsSuccessStatusCode)
                {
                    _logger.LogWarning("El endpoint PACS devolvió {Status} para la prestación {Id}.", checkRes.StatusCode, idPrestacion);
                    return null;
                }

                var contentType = checkRes.Content.Headers.ContentType?.MediaType ?? "";
                if (contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("El endpoint PACS devolvió JSON (recurso no encontrado) para la prestación {Id}.", idPrestacion);
                    return null;
                }

                return pacsUrl;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener la imagen de la prestación {Id}.", idPrestacion);
                return null;
            }
        }
    }
}
