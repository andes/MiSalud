using AndesServices.Entities;
using AndesServices.Interfaces;

namespace AndesServices.Services
{
    public class OrganizacionService : IOrganizacion
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrganizacionService(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Organizacion> ObtenerOrganizacionPorIdAsync(string token, string id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");
                client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                using (HttpResponseMessage res = await client.GetAsync($"core/tm/organizaciones/{id}"))
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
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener los turnos: {exception.Message}");
                return null;
            }
            return null;
        }
        
    }
}
