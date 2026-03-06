using AndesServices.DTOs;
using AndesServices.Entities;
using AndesServices.Interfaces;
using System.Net.Http;

namespace AndesServices.Services
{
    public class TerritorioService : ITerritorio
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TerritorioService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Provincia>> ObtenerProvinciasAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");

                using (HttpResponseMessage res = await client.GetAsync("core/tm/provincias"))
                {
                    res.EnsureSuccessStatusCode();

                    List<ProvinciaDto> provinciasDto = await res.Content.ReadFromJsonAsync<List<ProvinciaDto>>();

                    if (provinciasDto == null)
                    {
                        Console.WriteLine("No se encontraron provincias.");
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
                Console.WriteLine($"Error al obtener las provincias: {exception.Message}");
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
                var client = _httpClientFactory.CreateClient("Andes");
                var url = $"core/tm/localidades?provincia={Uri.EscapeDataString(idProvincia)}";
                
                if (!string.IsNullOrEmpty(nombre))
                {
                    url += $"&nombre={Uri.EscapeDataString(nombre)}";
                }

                using (HttpResponseMessage res = await client.GetAsync(url))
                {
                    res.EnsureSuccessStatusCode();

                    List<LocalidadDto> localidadesDto = await res.Content.ReadFromJsonAsync<List<LocalidadDto>>();

                    if (localidadesDto == null)
                    {
                        Console.WriteLine($"No se encontraron localidades para la provincia {idProvincia}.");
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
                Console.WriteLine($"Error al obtener las localidades para la provincia {idProvincia}: {exception.Message}");
                return new List<Localidad>();
            }
        }
    }
}
