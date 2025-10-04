using AndesServices.Entities;
using AndesServices.Interfaces;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace AndesServices.Services
{
    public class HistoriaSaludService : IHistoriaSalud
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HistoriaSaludService> _logger;
        public HistoriaSaludService(IConfiguration configuration
            , IHttpClientFactory httpClientFactory
            , ILogger<HistoriaSaludService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        private string GetServiciosBaseUrl()
        {
            var cfg = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(cfg);
            var baseUrl = cfg.usarProd ? cfg.UrlProyectoServiciosProd : cfg.UrlProyectoServiciosDemo;
            return baseUrl.TrimEnd('/');
        }

        // Implementación de los métodos de la interfaz IHistoriaSalud
        public async Task<List<CategoriaHistoriaSalud>> ObtenerCategoriasHistoriaSaludAsync(string token)
        {
            string url = GetServiciosBaseUrl() + "/modules/mobileApp/categoria";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    using (HttpResponseMessage res = await client.GetAsync(url))
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
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener las categorias: {exception.Message}");
                return null;
            }
            return null;
        }

        public async Task<List<PrestacionHistoriaSalud>> ObtenerPrestacionesAsync(string token, string tipoPrestaciones, string idPaciente, string estado = "validada")
        {
            string apiUrl = "";
            if (tipoPrestaciones == "90226004" || tipoPrestaciones == "86273004")
            {
                // CDA
                apiUrl = "/modules/cda/paciente/";
            }
            else
            {
                // RUP
                apiUrl = "/modules/rup/prestaciones";
            }

            string url = GetServiciosBaseUrl() + apiUrl;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);
                    string queryParams = "";
                    bool esCda = (tipoPrestaciones == "90226004" || tipoPrestaciones == "86273004");
                    if (esCda)
                    {
                        queryParams = idPaciente;
                    }
                    else
                    {
                        queryParams = "?tipoPrestaciones=" + tipoPrestaciones + "&idPaciente=" + idPaciente + "&estado=" + estado;
                    }

                    using (HttpResponseMessage res = await client.GetAsync(url + queryParams))
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
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener las prestaciones: {exception.Message}");
                return null;
            }
            return null;
        }

        public async Task<Byte[]> DescargarCDAFilePorIdAsync(string token, string id)
        {
            byte[] unByte = null;
            var baseUrl = GetServiciosBaseUrl();
            var url = $"{baseUrl}/modules/cda/{id}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);
                    //var parametrosBody = new StringContent("{\"protocolo\":{\"data\":{\"idProtocolo\":" + idProtocolo + ",\"documento\":" + documento + "}}}", System.Text.Encoding.UTF8, "application/json");
                    using (HttpResponseMessage res = await client.GetAsync(url))
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
    }
}
