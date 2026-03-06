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
        private readonly IHttpClientFactory _httpClientFactory;

        public VacunacionService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Task<bool> ActualizarVacunacionAsync(string idVacunacion, string vacuna, string fechaVacuna, string dosis)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarVacunacionAsync(string idVacunacion)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Vacunacion>> ObtenerCampañasVacunacion()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");

                using (HttpResponseMessage res = await client.GetAsync("modules/mobileApp/vacunas"))
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
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error");
                Console.WriteLine(exception.Message);
            }
            return null;
        }

        public Task<List<Vacunacion>> ObtenerVacunacionesPorDocumentoAsync(string documento)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegistrarVacunacionAsync(string documento, string vacuna, string fechaVacuna, string dosis)
        {
            throw new NotImplementedException();
        }
    }
}
