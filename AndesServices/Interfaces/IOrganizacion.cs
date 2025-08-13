using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IOrganizacion
    {
        Task<Organizacion> ObtenerOrganizacionPorIdAsync(string token, string id);
    }
}
