using AndesServices.DTOs.LaboratoriosRania;
using AndesServices.Entities;
using AndesServices.Interfaces;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AndesServices.Services
{
    public class MisLaboratoriosService : IMisLaboratorios
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MisLaboratoriosService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _andesClient, _lachybsClient, _xroadssClient;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public MisLaboratoriosService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<MisLaboratoriosService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _andesClient = httpClientFactory.CreateClient("Andes");
            _lachybsClient = httpClientFactory.CreateClient("LACHYBS_NOREDIRECT");
            _xroadssClient = httpClientFactory.CreateClient("ApiXroadssAndes");
        }

        public Task<bool> ActualizarLaboratorioAsync(string idProtocolo, string documento, string apellido, string nombre, string codigoHIV, string fechanacimiento, string sexobiologico, string numero, string fecha, string laboratorio, string medicoSolicitante, string efectorSolicitante, string origen, string tipoMuestra)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarLaboratorioAsync(string idProtocolo)
        {
            throw new NotImplementedException();
        }

        public Task<MisLaboratorios> ObtenerLaboratorioPorIdAsync(string idProtocolo)
        {
            throw new NotImplementedException();
        }

        public async Task<Byte[]> DescargarLaboratorioPorIdAsync(string idProtocolo, string documento)
        {
            byte[] unByte = null;

            try
            {
                var parametrosBody = new StringContent("{\"protocolo\":{\"data\":{\"idProtocolo\":" + idProtocolo + ",\"documento\":" + documento + "}}}", System.Text.Encoding.UTF8, "application/json");
                using (HttpResponseMessage res = await _andesClient.PostAsync("modules/descargas/laboratorio", parametrosBody))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        byte[]? fileResponse = await res.Content.ReadAsByteArrayAsync();
                        if (fileResponse == null)
                        {
                            _logger.LogWarning("El contenido del archivo es null para protocolo {IdProtocolo} y documento {Documento}", idProtocolo, documento);
                            return await Task.FromResult(unByte);
                        }

                        return fileResponse;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener el archivo del laboratorio para protocolo {IdProtocolo} y documento {Documento}", idProtocolo, documento);
                return await Task.FromResult(unByte);
            }
            return await Task.FromResult(unByte);
        }

        public async Task<Byte[]> DescargarLaboratorioCDAPorIdAsync(string documento)
        {
            byte[] unByte = null;

            try
            {
                using (HttpResponseMessage res = await _andesClient.GetAsync($"modules/cda/{documento}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        byte[]? fileResponse = await res.Content.ReadAsByteArrayAsync();
                        if (fileResponse == null)
                        {
                            _logger.LogWarning("El contenido del archivo es null para documento {Documento}", documento);
                            return await Task.FromResult(unByte);
                        }

                        return fileResponse;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener el archivo del laboratorio para documento {Documento}", documento);
                return await Task.FromResult(unByte);
            }
            return await Task.FromResult(unByte);
        }

        public async Task<string> DescargarLaboratorioLACHyBSPorIdAsync(string usuario, string clave, string idProtocolo)
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
                var client = _lachybsClient;
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
                    _logger.LogError("Descargar informe LACHYBS fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                    return null;
                }

                var informe = System.Text.Json.JsonSerializer.Deserialize<LaboratorioLachybsInforme>(body, JsonOpts);
                if (informe == null || string.IsNullOrWhiteSpace(informe.informe_url))
                {
                    _logger.LogWarning("No se obtuvo informe_url para protocolo {Id}", idProtocolo);
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

        public async Task<List<MisLaboratorios>> ObtenerMisLaboratoriosAsync(string pacienteId, string fechaDde, string fechaHta)
        {
            try
            {

                using (HttpResponseMessage res = await _andesClient.GetAsync($"modules/rup/protocolosLab?pacienteId={pacienteId}&fechaDde={fechaDde}&fechaHta={fechaHta}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        string jsonString = await res.Content.ReadAsStringAsync();
                        var root = JToken.Parse(jsonString);

                        // Desanidar si vino como string con JSON dentro
                        if (root.Type == JTokenType.String)
                        {
                            root = JToken.Parse(root.Value<string>());
                        }

                        // Soportar objeto o array con propiedad Data
                        JToken data = root.Type == JTokenType.Array ? root[0]?["Data"] : root["Data"];
                        var listaLaboratorios = data?.ToObject<List<MisLaboratorios>>();

                        if (listaLaboratorios == null)
                        {
                            _logger.LogError("Respuesta sin Data. Body: {Body}", jsonString);
                            return null;
                        }

                        // Normalizar fechas a formato dd/MM/yyyy
                        foreach (var laboratorio in listaLaboratorios)
                        {
                            if (!string.IsNullOrEmpty(laboratorio.fecha))
                            {
                                laboratorio.fecha = NormalizarFecha(laboratorio.fecha);
                            }
                        }

                        return listaLaboratorios;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener los laboratorios");
                return null;
            }
            return null;
        }

        /// <summary>
        /// Normaliza una fecha a formato dd/MM/yyyy desde cualquier formato reconocible
        /// </summary>
        private string NormalizarFecha(string fecha)
        {
            if (string.IsNullOrWhiteSpace(fecha))
                return fecha;

            try
            {
                // Intentar parsear en formato dd/MM/yyyy (ya está en el formato correcto)
                if (DateTime.TryParseExact(fecha, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime resultado))
                {
                    return resultado.ToString("dd/MM/yyyy");
                }

                // Intentar parsear en formato ISO (yyyyMMdd)
                if (DateTime.TryParseExact(fecha, "yyyyMMdd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out resultado))
                {
                    return resultado.ToString("dd/MM/yyyy");
                }

                // Intentar parsear en formato yyyy-MM-dd
                if (DateTime.TryParseExact(fecha, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out resultado))
                {
                    return resultado.ToString("dd/MM/yyyy");
                }

                // Intentar parsear con el parseador general
                if (DateTime.TryParse(fecha, out resultado))
                {
                    return resultado.ToString("dd/MM/yyyy");
                }

                // Si no se pudo parsear, devolver la fecha original
                _logger.LogWarning("No se pudo parsear la fecha: {Fecha}", fecha);
                return fecha;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al normalizar la fecha: {Fecha}", fecha);
                return fecha;
            }
        }

        public async Task<List<MisLaboratoriosCDA>> ObtenerMisLaboratoriosCDAAsync(string pacienteId, string fechaDde, string fechaHta)
        {
            try
            {
                using (HttpResponseMessage res = await _andesClient.GetAsync($"modules/cda/paciente/{pacienteId}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        List<MisLaboratoriosCDA?> misLaboratorios = new List<MisLaboratoriosCDA?>();

                        misLaboratorios = await res.Content.ReadFromJsonAsync<List<MisLaboratoriosCDA>>();

                        if (misLaboratorios == null)
                        {
                            return null;
                        }

                        return misLaboratorios;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener los laboratorios CDA para paciente {PacienteId}", pacienteId);
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
                var client = _lachybsClient;
                client.DefaultRequestHeaders.Authorization = BuildBasicAuthHeader(usuario?.Trim(), clave?.Trim());
                if (!client.DefaultRequestHeaders.Accept.Any())
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!client.DefaultRequestHeaders.UserAgent.Any())
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("SaludPortalClient/1.0");

                var res = await client.GetAsync(url);
                var redirectUri = res.Headers.Location.IsAbsoluteUri
                    ? res.Headers.Location
                    : new Uri(new Uri(url), res.Headers.Location);

                var secondReq = new HttpRequestMessage(HttpMethod.Get, redirectUri);
                secondReq.Headers.Authorization = BuildBasicAuthHeader(usuario?.Trim(), clave?.Trim());
                secondReq.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!secondReq.Headers.UserAgent.Any())
                    secondReq.Headers.UserAgent.ParseAdd("SaludPortalClient/1.0");

                res.Dispose();
                res = await client.SendAsync(secondReq);
                var body = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogError("LACHYBS fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                    return null;
                }

                var lista = JsonSerializer.Deserialize<List<LaboratoriosLachybs>>(body, JsonOpts);
                if (lista == null || lista.Count == 0)
                {
                    return null;
                }

                // Normalizar fechas a formato dd/MM/yyyy
                foreach (var laboratorio in lista)
                {
                    if (!string.IsNullOrEmpty(laboratorio.fecha))
                    {
                        laboratorio.fecha = NormalizarFecha(laboratorio.fecha);
                    }
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

        public Task<List<MisLaboratorios>> ObtenerMisLaboratoriosAsync(string documento)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegistrarLaboratorioAsync(string idProtocolo, string documento, string apellido, string nombre, string codigoHIV, string fechanacimiento, string sexobiologico, string numero, string fecha, string laboratorio, string medicoSolicitante, string efectorSolicitante, string origen, string tipoMuestra)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProtocoloRaniaResponseDto>> ObtenerProtocolosRania(string dni)
        {
            try
            {
                using (HttpResponseMessage res = await _xroadssClient.GetAsync($"r1/OPTIC/COM/COM00007/GP-LABRANIA/protocolo?dni={dni}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var lista = await res.Content.ReadFromJsonAsync<List<ProtocoloRaniaResponseDto>>(JsonOpts);

                        if (lista == null)
                        {
                            return [];
                        }

                        // Normalizar fechas a formato dd/MM/yyyy
                        foreach (var protocolo in lista)
                        {
                            if (!string.IsNullOrEmpty(protocolo.Fecha))
                            {
                                protocolo.Fecha = NormalizarFecha(protocolo.Fecha);
                            }
                        }

                        return lista;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener los laboratorios Rania para el DNI {Dni}", dni);
                return [];
            }
            return [];
        }

        public async Task<InformeRaniaResponseDto?> ObtenerInformeRania(string protocoloId)
        {
            if (string.IsNullOrWhiteSpace(protocoloId))
            {
                _logger.LogWarning("Protocolo ID no proporcionado en ObtenerInformeRania");
                return null;
            }

            try
            {
                using (HttpResponseMessage res = await _xroadssClient.GetAsync($"r1/OPTIC/COM/COM00007/GP-LABRANIA/informe?protocolo_id={protocoloId}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var informe = await res.Content.ReadFromJsonAsync<InformeRaniaResponseDto>(JsonOpts);

                        if (informe == null)
                        {
                            return null;
                        }

                        return informe;
                    }
                    else
                    {
                        var body = await res.Content.ReadAsStringAsync();
                        _logger.LogWarning("Error al obtener informe Rania. Status: {StatusCode} Body: {Body} para protocolo {ProtocoloId}",
                            res.StatusCode, body, protocoloId);
                        return null;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener el informe Rania para protocolo {ProtocoloId}", protocoloId);
                return null;
            }
        }

        public async Task<Byte[]> DescargarInformeRaniaPorIdAsync(string protocoloId)
        {
            if (string.IsNullOrWhiteSpace(protocoloId))
            {
                _logger.LogWarning("Protocolo ID no proporcionado en DescargarInformeRaniaPorIdAsync");
                return null;
            }

            try
            {
                // Obtener la URL del informe
                var informeResponse = await ObtenerInformeRania(protocoloId);

                if (informeResponse == null || string.IsNullOrWhiteSpace(informeResponse.InformeUrl))
                {
                    _logger.LogWarning("No se pudo obtener la URL del informe para protocolo {ProtocoloId}", protocoloId);
                    return null;
                }

                // Descargar el contenido desde la URL
                var client = _httpClientFactory.CreateClient();

                using (HttpResponseMessage res = await client.GetAsync(informeResponse.InformeUrl))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        byte[]? fileResponse = await res.Content.ReadAsByteArrayAsync();

                        if (fileResponse == null)
                        {
                            _logger.LogWarning("El contenido del archivo es null para protocolo {ProtocoloId}", protocoloId);
                            return null;
                        }

                        return fileResponse;
                    }
                    else
                    {
                        _logger.LogWarning("Error al descargar informe Rania. Status: {StatusCode} para protocolo {ProtocoloId}",
                            res.StatusCode, protocoloId);
                        return null;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al descargar el informe Rania para protocolo {ProtocoloId}", protocoloId);
                return null;
            }
        }
    }
}
