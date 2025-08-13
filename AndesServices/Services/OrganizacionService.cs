using AndesServices.Entities;
using AndesServices.Interfaces;

namespace AndesServices.Services
{
    public class OrganizacionService : IOrganizacion
    {
        private IConfiguration? _configuration { get; }
        private readonly IHttpClientFactory _httpClientFactory;

        public OrganizacionService(IConfiguration? configuration, IHttpClientFactory? httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Organizacion> ObtenerOrganizacionPorIdAsync(string token, string id)
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/core/tm/organizaciones"
                : conexionServicios.UrlProyectoServiciosDemo + "/core/tm/organizaciones";

            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                    using (HttpResponseMessage res = await client.GetAsync(url + "/" + id))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            Organizacion organizacion = await res.Content.ReadFromJsonAsync<Organizacion>();
                            if (organizacion == null)
                            {
                                Console.WriteLine("No se encontró la organización.");
                                return null;
                            }

                            return organizacion;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener los turnos: {exception.Message}");
                return null;
            }
            return null;
        }
        
    }
}
