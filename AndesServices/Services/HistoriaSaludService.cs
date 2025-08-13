using AndesServices.Entities;
using AndesServices.Interfaces;
using Newtonsoft.Json.Linq;

namespace AndesServices.Services
{
    public class HistoriaSaludService : IHistoriaSalud
    {
        private readonly IConfiguration _configuration;
        public HistoriaSaludService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // Implementación de los métodos de la interfaz IHistoriaSalud
        public async Task<List<CategoriaHistoriaSalud>> ObtenerCategoriasHistoriaSaludAsync(string token)
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/categoria"
                : conexionServicios.UrlProyectoServiciosDemo + "/categoria";

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
    }
}
