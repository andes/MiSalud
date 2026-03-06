using AndesServices.Entities;
using AndesServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace AndesServices.Services
{
    public class MisRecetasService : IMisRecetas
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MisRecetasService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Task<bool> ActualizarEstadoRecetaAsync(string recetaId, Estado nuevoEstado)
        {
            throw new NotImplementedException();
        }

        public Task<MisReceta> ObtenerRecetaPorIdAsync(string recetaId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MisReceta>> ObtenerRecetasPacienteAsync(string pacienteId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");

                string estado = "sin-dispensa,dispensada,dispensa-parcial";
                //string queryParams = "?estado=" + estado + "&dni=" + dni + "&fecNac=" + fecNac + "&apellido=" + apellido + "&fechaDde=" + fechaDde + "&fechaHta=" + fechaHta;
                string url = $"modules/recetas?pacienteId={pacienteId}&estadoDispensa={estado}";

                using (HttpResponseMessage res = await client.GetAsync(url))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        List<MisReceta?> LstMisReceta = await res.Content.ReadFromJsonAsync<List<MisReceta>>();
                        if (LstMisReceta == null)
                        {
                            Console.WriteLine("No se encontraron recetas disponibles.");
                            return null;
                        }

                        return LstMisReceta;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener las recetas: {exception.Message}");
                return null;
            }
            return null;
        }
    }
}
