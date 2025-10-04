using AndesServices.Entities;
using AndesServices.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace AndesServices.Services
{
    public class MisLaboratoriosService : IMisLaboratorios
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MisLaboratoriosService> _logger;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public MisLaboratoriosService(IConfiguration configuration
            , IHttpClientFactory httpClientFactory
            , ILogger<MisLaboratoriosService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public Task<bool> ActualizarLaboratorioAsync(string token, string idProtocolo, string documento, string apellido, string nombre, string codigoHIV, string fechanacimiento, string sexobiologico, string numero, string fecha, string laboratorio, string medicoSolicitante, string efectorSolicitante, string origen, string tipoMuestra)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarLaboratorioAsync(string token, string idProtocolo)
        {
            throw new NotImplementedException();
        }

        public Task<MisLaboratorios> ObtenerLaboratorioPorIdAsync(string token, string idProtocolo)
        {
            throw new NotImplementedException();
        }

        public async Task<Byte[]> DescargarLaboratorioPorIdAsync(string token, string idProtocolo, string documento)
        {
            byte[] unByte = null;
            var baseUrl = GetServiciosBaseUrl();
            var url = $"{baseUrl}/modules/descargas/laboratorio";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);
                    var parametrosBody = new StringContent("{\"protocolo\":{\"data\":{\"idProtocolo\":" + idProtocolo + ",\"documento\":" + documento + "}}}", System.Text.Encoding.UTF8, "application/json");
                    using (HttpResponseMessage res = await client.PostAsync(url, parametrosBody))
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
                Console.WriteLine($"Error al obtener el archivo del laboratorio: {exception.Message}");
                return await Task.FromResult(unByte);
            }
            return await Task.FromResult(unByte);
        }

        public async Task<string> DescargarLaboratorioLACHyBSPorIdAsync(string usuario, string clave,string idProtocolo)
        {
            if (string.IsNullOrWhiteSpace(idProtocolo))
            {
                _logger.LogWarning("Id de protocolo vacío en DescargarLaboratorioLACHyBSPorIdAsync");
                return null;
            }

            var baseUrl = GetLachybsBaseUrl();
            var url = $"{baseUrl}:6041/informe?protocolo_id={idProtocolo}";

            try
            {
                var client = _httpClientFactory.CreateClient("LACHYBS_NOREDIRECT");
                client.DefaultRequestHeaders.Authorization = BuildBasicAuthHeader(usuario?.Trim(), clave?.Trim());
                if (!client.DefaultRequestHeaders.Accept.Any())
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!client.DefaultRequestHeaders.UserAgent.Any())
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("SaludPortalClient/1.0");

                var res = await client.GetAsync(url);
                var body = await res.Content.ReadAsStringAsync();

                // Manejo de redirect preservando headers
                if ((int)res.StatusCode is 301 or 302 or 307 or 308 && res.Headers.Location != null)
                {
                    var redirectUri = res.Headers.Location.IsAbsoluteUri
                        ? res.Headers.Location
                        : new Uri(new Uri(url), res.Headers.Location);

                    res.Dispose();

                    using var secondReq = new HttpRequestMessage(HttpMethod.Get, redirectUri);
                    CopyDefaultHeadersFromClient(client, secondReq);
                    res = await client.SendAsync(secondReq);
                    body = await res.Content.ReadAsStringAsync();
                }

                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Descargar informe LACHYBS fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                    return null;
                }

                var informe = System.Text.Json.JsonSerializer.Deserialize<LaboratorioLachybsInforme>(body, JsonOpts);
                if (informe == null || string.IsNullOrWhiteSpace(informe.informe_url))
                {
                    _logger.LogInformation("No se obtuvo informe_url para protocolo {Id}", idProtocolo);
                    return null;
                }

                return informe.informe_url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo informe_url de protocolo {Id}", idProtocolo);
                return null;
            }
        }

        public async Task<List<MisLaboratorios>> ObtenerMisLaboratoriosAsync(string token, string pacienteId, string fechaDde, string fechaHta)
        {
            var baseUrl = GetServiciosBaseUrl();
            var url = $"{baseUrl}/modules/rup/protocolosLab?pacienteId={pacienteId}&fechaDde={fechaDde}&fechaHta={fechaHta}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    string queryParams = "?pacienteId=" + pacienteId + "&fechaDde=" + fechaDde + "&fechaHta=" + fechaHta;

                    using (HttpResponseMessage res = await client.GetAsync(url + queryParams))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            List<MisLaboratorios?> misLaboratorios = new List<MisLaboratorios?>();

                            string jsonString = await res.Content.ReadAsStringAsync();
                            var rootArray = JArray.Parse(jsonString);
                            var dataToken = rootArray[0]["Data"];
                            List<MisLaboratorios>? listaLaboratorios = dataToken?.ToObject<List<MisLaboratorios>>();

                            if (listaLaboratorios == null)
                            {
                                Console.WriteLine("No se encontraron laboratorios.");
                                return null;
                            }

                            return listaLaboratorios;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener los laboratorios: {exception.Message}");
                return null;
            }
            return null;
        }

        public async Task<List<LaboratoriosLachybs>> ObtenerMisLaboratoriosLACHYBSAsync(string usuario, string clave, string documento)
        {
            var baseUrl = GetLachybsBaseUrl();
            var url = $"{baseUrl}:6040/protocolo?dni={documento}";

            try
            {
                var client = _httpClientFactory.CreateClient("LACHYBS_NOREDIRECT"); // Asegura su registro (AllowAutoRedirect = false opcional)
                client.DefaultRequestHeaders.Authorization = BuildBasicAuthHeader(usuario?.Trim(), clave?.Trim());
                if (!client.DefaultRequestHeaders.Accept.Any())
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!client.DefaultRequestHeaders.UserAgent.Any())
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("SaludPortalClient/1.0");

                var res = await client.GetAsync(url);
                //var body = await res.Content.ReadAsStringAsync();
                //if ((int)res.StatusCode is 301 or 302 or 307 or 308)
                //{
                    // Posible redirect (http→https?). Preserva Authorization manualmente
                    var redirectUri = res.Headers.Location.IsAbsoluteUri
                        ? res.Headers.Location
                        : new Uri(new Uri(url), res.Headers.Location);

                    Console.WriteLine($"[LACHYBS] Following redirect to {redirectUri}");

                    var secondReq = new HttpRequestMessage(HttpMethod.Get, redirectUri);
                    secondReq.Headers.Authorization = BuildBasicAuthHeader(usuario?.Trim(), clave?.Trim());
                    secondReq.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (!secondReq.Headers.UserAgent.Any())
                        secondReq.Headers.UserAgent.ParseAdd("SaludPortalClient/1.0");

                    res.Dispose();
                    res = await client.SendAsync(secondReq);
                    var body = await res.Content.ReadAsStringAsync();
                    Console.WriteLine($"[LACHYBS] 2nd Status {(int)res.StatusCode} {res.ReasonPhrase}");
                //}

                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogWarning("LACHYBS fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                    return null;
                }

                var lista = JsonSerializer.Deserialize<List<LaboratoriosLachybs>>(body, JsonOpts);
                if (lista == null || lista.Count == 0)
                {
                    _logger.LogInformation("LACHYBS sin resultados para dni {Documento}", documento);
                    return null;
                }
                return lista;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo LACHYBS dni {Documento}", documento);
                return null;
            }
        }
        private static void CopyDefaultHeadersFromClient(HttpClient client, HttpRequestMessage targetRequest)
        {
            if (!targetRequest.Headers.Accept.Any())
            {
                foreach (var a in client.DefaultRequestHeaders.Accept)
                    targetRequest.Headers.Accept.Add(a);
            }
            if (!targetRequest.Headers.UserAgent.Any())
            {
                foreach (var ua in client.DefaultRequestHeaders.UserAgent)
                    targetRequest.Headers.UserAgent.Add(ua);
            }
            if (client.DefaultRequestHeaders.Authorization != null)
                targetRequest.Headers.Authorization = client.DefaultRequestHeaders.Authorization;
        }
        private string GetServiciosBaseUrl()
        {
            var cfg = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(cfg);
            var baseUrl = cfg.usarProd ? cfg.UrlProyectoServiciosProd : cfg.UrlProyectoServiciosDemo;
            return baseUrl.TrimEnd('/');
        }

        private string GetLachybsBaseUrl()
        {
            var cfg = new ConexionLACHyBS();
            _configuration.GetSection("urlLASCHyBS").Bind(cfg);
            var baseUrl = cfg.usarProd ? cfg.UrlLACHYBSProd : cfg.UrlLACHYBSDemo;
            return baseUrl.TrimEnd('/');
        }

        private static AuthenticationHeaderValue BuildBasicAuthHeader(string usuario, string clave)
        {
            // Evita nulls
            usuario ??= string.Empty;
            clave ??= string.Empty;
            var raw = $"{usuario}:{clave}";
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
            return new AuthenticationHeaderValue("Basic", base64);
        }

        public Task<List<MisLaboratorios>> ObtenerMisLaboratoriosAsync(string token, string documento)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegistrarLaboratorioAsync(string token, string idProtocolo, string documento, string apellido, string nombre, string codigoHIV, string fechanacimiento, string sexobiologico, string numero, string fecha, string laboratorio, string medicoSolicitante, string efectorSolicitante, string origen, string tipoMuestra)
        {
            throw new NotImplementedException();
        }
    }
}
