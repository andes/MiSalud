using AndesServices.DTOs;
using AndesServices.Entities;
using AndesServices.Interfaces;

namespace AndesServices.Services
{
    public class TerritorioService : ITerritorio
    {
        private readonly HttpClient _andesClient;
        private readonly ILogger<TerritorioService> _logger;

        public TerritorioService(IHttpClientFactory httpClientFactory, ILogger<TerritorioService> logger)
        {
            _andesClient = httpClientFactory.CreateClient("Andes");
            _logger = logger;
        }

        public async Task<List<Provincia>> ObtenerProvinciasAsync()
        {
            try
            {
                using (HttpResponseMessage res = await _andesClient.GetAsync("core/tm/provincias"))
                {
                    res.EnsureSuccessStatusCode();

                    List<ProvinciaDto> provinciasDto = await res.Content.ReadFromJsonAsync<List<ProvinciaDto>>();

                    if (provinciasDto == null)
                    {
                        _logger.LogWarning("No se encontraron provincias.");
                        return new List<Provincia>();
                    }

                    // Mapear DTOs a entidades
                    return provinciasDto.Select(p => new Provincia
                    {
                        _id = p._id,
                        id = p.id,
                        nombre = p.nombre
                    }).ToList();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener las provincias.");
                return new List<Provincia>();
            }
        }

        public async Task<List<Localidad>> ObtenerLocalidadesPorProvinciaAsync(string idProvincia, string? nombre = null)
        {
            if (string.IsNullOrEmpty(idProvincia))
            {
                return new List<Localidad>();
            }

            try
            {
                var url = $"core/tm/localidades?provincia={Uri.EscapeDataString(idProvincia)}";
                
                if (!string.IsNullOrEmpty(nombre))
                {
                    url += $"&nombre={Uri.EscapeDataString(nombre)}";
                }

                using (HttpResponseMessage res = await _andesClient.GetAsync(url))
                {
                    res.EnsureSuccessStatusCode();

                    List<LocalidadDto> localidadesDto = await res.Content.ReadFromJsonAsync<List<LocalidadDto>>();

                    if (localidadesDto == null)
                    {
                        return new List<Localidad>();
                    }

                    // Mapear DTOs a entidades
                    return localidadesDto.Select(l => new Localidad
                    {
                        _id = l._id ?? string.Empty,
                        nombre = l.nombre ?? string.Empty,
                    }).ToList();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Error al obtener las localidades para la provincia {idProvincia}.");
                return new List<Localidad>();
            }
        }
    }
}
