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
                ? conexionServicios.UrlProyectoServiciosProd + "/modules/mobileApp/categoria"
                : conexionServicios.UrlProyectoServiciosDemo + "/modules/mobileApp/categoria";

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

        public async Task<List<PrestacionHistoriaSalud>> ObtenerPrestacionesAsync(string token, string tipoPrestaciones, string idPaciente, string estado = "validada")
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/modules/rup/prestaciones"
                : conexionServicios.UrlProyectoServiciosDemo + "/modules/rup/prestaciones";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);
                    string queryParams = "?tipoPrestaciones=" + tipoPrestaciones + "&idPaciente=" + idPaciente + "&estado=" + estado;

                    using (HttpResponseMessage res = await client.GetAsync(url + queryParams))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            try
                            {
                                List<PrestacionHistoriaSalud>? listaPrestacionesHistoriaSalud = await res.Content.ReadFromJsonAsync<List<PrestacionHistoriaSalud>>();

                                if (listaPrestacionesHistoriaSalud == null)
                                {
                                    Console.WriteLine("No se encontraron prestaciones.");
                                    return null;
                                }

                                return listaPrestacionesHistoriaSalud;
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener las prestaciones: {exception.Message}");
                return null;
            }
            return null;
        }
    }
}
