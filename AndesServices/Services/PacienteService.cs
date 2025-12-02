using AndesServices.Entities;
using AndesServices.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AndesServices.Services
{
    public class PacienteService : IPaciente
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public PacienteService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
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

        public async Task<Paciente> ObtenerPacientePorIdAsync(string token, string idPaciente)
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/modules/mobileApp/paciente"
                : conexionServicios.UrlProyectoServiciosDemo + "/modules/mobileApp/paciente";

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token no proporcionado.");
            }
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    using (HttpResponseMessage res = await client.GetAsync(url + "/" + idPaciente))
                    {
                        res.EnsureSuccessStatusCode();

                        Paciente unPaciente = await res.Content.ReadFromJsonAsync<Paciente>();

                        if (unPaciente == null)
                        {
                            Console.WriteLine("No se encontraron recetas disponibles.");
                            return null;
                        }

                        return unPaciente;
                    }

                }
            }

            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener los datos del paciente: {exception.Message}");
                return null;
            }
        }

        public async Task<userLocation> ObtenerGeoreferenciaPaciente(string direccion)
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/modules/georeferencia/georeferenciar"
                : conexionServicios.UrlProyectoServiciosDemo + "/modules/georeferencia/georeferenciar";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(url + "?direccion=" + direccion),
                    };

                    using (HttpResponseMessage res = await client.SendAsync(request))
                    {
                        res.EnsureSuccessStatusCode();

                        userLocation unaGeoreferencia = await res.Content.ReadFromJsonAsync<userLocation>();

                        if (unaGeoreferencia == null)
                        {
                            Console.WriteLine("No se encontraron recetas disponibles.");
                            return null;
                        }

                        return unaGeoreferencia;
                    }
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener los datos del paciente: {exception.Message}");
                return null;
            }
        }

        // Modifica datos personales de un Paciente
        //public async Task<Paciente> ModificarDatos(string token, string idPaciente, string nombreAutopercibido, string genero, Direccion domicilio, Contacto contacto)
        //{
        //    string url = GetServiciosBaseUrl() + "/modules/mobileApp/pacientes/idPaciente";
        // //“genero”: 'mujer', 'mujer trans', 'varon', 'varon trans', 'no binario', 'travesti', 'masculinidad trans', 'femenino', 'masculino', 'otro'
        //    try
        //    {
        //        using (HttpClient client = _httpClientFactory.CreateClient())
        //        {
        //            client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

        //            Paciente pacienteModificado = await ObtenerPacientePorIdAsync(token, idPaciente);
        //            {
        //                pacienteModificado.nombreCorrectoReportado = nombreAutopercibido;
        //                pacienteModificado.genero = genero;

        //                if (pacienteModificado.direccion != null)
        //                {
        //                    pacienteModificado.direccion.RemoveAll(d => d.id == domicilio.id);
        //                }
        //                pacienteModificado.direccion.Add(domicilio);

        //                pacienteModificado.contacto.Where(c=> c.id == contacto.id);
                        
        //                pacienteModificado.contactoTelefono = contactoTelefono;
        //            }

        //            var request = new HttpRequestMessage
        //            {
        //                Method = HttpMethod.Patch,
        //                RequestUri = new Uri(url),
        //                Content = new StringContent(JsonConvert.SerializeObject(new
        //                {
        //                    idAgenda,
        //                    idBloque,
        //                    idTurno,
        //                    paciente,
        //                    tipoPrestacion,
        //                    tipoTurno,
        //                    emitidoPor,
        //                    nota,
        //                    motivoConsulta
        //                }), System.Text.Encoding.UTF8, "application/json")
        //            };

        //            using (HttpResponseMessage res = await client.SendAsync(request))
        //            {
        //                if (res.IsSuccessStatusCode)
        //                {
        //                    Console.WriteLine("Turno confirmado.");
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine($"Error al registrar el turno: {exception.Message}");
        //    }
        //    return false; // Ensure a boolean is returned in case of failure
        //}
    }
}
