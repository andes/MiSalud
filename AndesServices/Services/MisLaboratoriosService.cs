using AndesServices.Entities;
using AndesServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.Json;

namespace AndesServices.Services
{
    public class MisLaboratoriosService : IMisLaboratorios
    {
        private readonly IConfiguration _configuration;

        public MisLaboratoriosService(IConfiguration configuration)
        {
            _configuration = configuration;
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
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/descargas/laboratorio"
                : conexionServicios.UrlProyectoServiciosDemo + "/descargas/laboratorio";

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
                Console.WriteLine($"Error al obtener los turnos: {exception.Message}");
                return await Task.FromResult(unByte);
            }
            return await Task.FromResult(unByte);
        }

        public async Task<List<MisLaboratorios>> ObtenerMisLaboratoriosAsync(string token, string pacienteId, string fechaDde, string fechaHta)
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/rup/protocolosLab"
                : conexionServicios.UrlProyectoServiciosDemo + "/rup/protocolosLab";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    //string queryParams = "?pacienteId=67fe623a589d44b5a4c91063&fechaDde=" + fechaDde + "&fechaHta=" + fechaHta;
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

                            //MisLaboratoriosResponse? misLaboratoriosResp = JsonConvert.DeserializeObject<MisLaboratoriosResponse>(result);

                            //MisLaboratoriosResponse? misLaboratoriosResp = await res.Content.ReadFromJsonAsync<MisLaboratoriosResponse>();

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
