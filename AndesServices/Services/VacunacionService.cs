using AndesServices.Entities;
using AndesServices.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace AndesServices.Services
{
    public class VacunacionService : IVacunacion
    {
        private IConfiguration? _configuration { get; }
        public VacunacionService(IConfiguration? configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> ActualizarVacunacionAsync(string token, string idVacunacion, string vacuna, string fechaVacuna, string dosis)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarVacunacionAsync(string token, string idVacunacion)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Vacunacion>> ObtenerCampañasVacunacion(string token)
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/modules/mobileApp/vacunas"
                : conexionServicios.UrlProyectoServiciosDemo + "/modules/mobileApp/vacunas";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    using (HttpResponseMessage res = await client.GetAsync(url))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            List<Vacunacion?> vacunacion = await res.Content.ReadFromJsonAsync<List<Vacunacion>>();
                            if (vacunacion == null)
                            {
                                Console.WriteLine("No se encontraron campañas de vacunación.");
                                return null;
                            }

                            return vacunacion;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error");
                Console.WriteLine(exception.Message);
            }
            return null;
        }

        public Task<List<Vacunacion>> ObtenerVacunacionesPorDocumentoAsync(string token, string documento)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegistrarVacunacionAsync(string token, string documento, string vacuna, string fechaVacuna, string dosis)
        {
            throw new NotImplementedException();
        }
    }
}
