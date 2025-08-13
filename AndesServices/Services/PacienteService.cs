using AndesServices.Entities;
using AndesServices.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace AndesServices.Services
{
    public class PacienteService : IPaciente
    {
        private readonly IConfiguration _configuration;

        public PacienteService(IConfiguration configuration)
        {
            _configuration = configuration;
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

                    using (HttpResponseMessage res = await client.GetAsync(url + "/"+ idPaciente))
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
                ? conexionServicios.UrlProyectoServiciosProd + "/georeferencia/georeferenciar"
                : conexionServicios.UrlProyectoServiciosDemo + "/georeferencia/georeferenciar";

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
    }
}
