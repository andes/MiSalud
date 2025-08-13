using AndesServices.Entities;
using AndesServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace AndesServices.Services
{
    public class MisRecetasService : IMisRecetas
    {
        private readonly IConfiguration _configuration;

        public MisRecetasService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> ActualizarEstadoRecetaAsync(string token, string recetaId, Estado nuevoEstado)
        {
            throw new NotImplementedException();
        }

        public Task<MisReceta> ObtenerRecetaPorIdAsync(string token, string recetaId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MisReceta>> ObtenerRecetasPacienteAsync(string token, string pacienteId)
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/recetas"
                : conexionServicios.UrlProyectoServiciosDemo + "/recetas";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    string estado = "sin-dispensa,dispensada,dispensa-parcial";
                    //string queryParams = "?estado=" + estado + "&dni=" + dni + "&fecNac=" + fecNac + "&apellido=" + apellido + "&fechaDde=" + fechaDde + "&fechaHta=" + fechaHta;
                    string queryParams = "?pacienteId=" + pacienteId + "&estadoDispensa=" + estado;

                    using (HttpResponseMessage res = await client.GetAsync(url + queryParams))
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
