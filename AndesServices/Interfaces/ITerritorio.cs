using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface ITerritorio
    {
        Task<List<Provincia>> ObtenerProvinciasAsync(string token);
        Task<List<Localidad>> ObtenerLocalidadesPorProvinciaAsync(string token, string idProvincia, string? nombre = null);
    }
}
