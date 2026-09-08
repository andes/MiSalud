using AndesServices.Entities;
using AndesServices.Interfaces;

namespace AndesServices.Services
{
    public class OrganizacionService : IOrganizacion
    {
        private readonly ILogger<OrganizacionService> _logger;
        private readonly HttpClient _andesClient;

        public OrganizacionService(IHttpClientFactory httpClientFactory, ILogger<OrganizacionService> logger)
        {
            _logger = logger;
            _andesClient = httpClientFactory.CreateClient("Andes");
        }
        public async Task<Organizacion> ObtenerOrganizacionPorIdAsync(string id)
        {
            try
            {
                using (HttpResponseMessage res = await _andesClient.GetAsync($"core/tm/organizaciones/{id}"))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        Organizacion organizacion = await res.Content.ReadFromJsonAsync<Organizacion>();
                        if (organizacion == null)
                        {
                            string body = await res.Content.ReadAsStringAsync();
                            _logger.LogError("Obtener organizacion por id fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                            return null;
                        }

                        return organizacion;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener la organizacion por id");
                return null;
            }
            return null;
        }
        
    }
}
