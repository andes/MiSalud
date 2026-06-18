using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.Recetas;

namespace SaludPortal.Application.UseCases.Recetas;

public class ObtenerRecetasUseCase
{
    private readonly IMisRecetas _recetasService;

    public ObtenerRecetasUseCase(IMisRecetas recetasService)
    {
        _recetasService = recetasService;
    }

    public async Task<List<Receta>> EjecutarAsync(string pacienteId)
    {
        var recetas = await _recetasService.ObtenerRecetasPacienteAsync(pacienteId);
        if (recetas == null) return [];
        return [.. recetas.Select(r => r.MapToRecetaModel())];
    }
}
