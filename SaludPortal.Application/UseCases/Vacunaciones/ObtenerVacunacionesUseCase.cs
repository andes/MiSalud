using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.Vacunaciones;

namespace SaludPortal.Application.UseCases.Vacunaciones;

public class ObtenerVacunacionesUseCase
{
    private readonly IVacunacion _vacunacionService;

    public ObtenerVacunacionesUseCase(IVacunacion vacunacionService)
    {
        _vacunacionService = vacunacionService;
    }

    public async Task<List<VacunacionModel>> EjecutarAsync()
    {
        var vacunas = await _vacunacionService.ObtenerCampañasVacunacion();
        if (vacunas == null) return [];
        return [.. vacunas.Select(v => v.MapToVacunacionModel())];
    }
}
