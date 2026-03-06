using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface ITerritorio
    {
        Task<List<Provincia>> ObtenerProvinciasAsync();
        Task<List<Localidad>> ObtenerLocalidadesPorProvinciaAsync(string idProvincia, string? nombre = null);
    }
}
