using SaludPortal.Application.DTOs.Vacunaciones;

namespace AndesServices.Interfaces
{
    public interface IVacunacion
    {
        Task<List<VacunacionDto>> ObtenerCampañasVacunacion();
    }
}
