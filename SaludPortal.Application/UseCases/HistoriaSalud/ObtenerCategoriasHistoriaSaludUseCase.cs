using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.HistoriaSalud;

namespace SaludPortal.Application.UseCases.HistoriaSalud;

public class ObtenerCategoriasHistoriaSaludUseCase
{
    private readonly IHistoriaSalud _historiaSaludService;

    public ObtenerCategoriasHistoriaSaludUseCase(IHistoriaSalud historiaSaludService)
    {
        _historiaSaludService = historiaSaludService;
    }

    public async Task<List<CategoriaHistoriaSaludModel>> EjecutarAsync(string? expresionSnomed = null)
    {
        var categorias = await _historiaSaludService.ObtenerCategoriasHistoriaSaludAsync(expresionSnomed);
        if (categorias == null) return [];
        return [.. categorias.Select(c => c.MapToCategoriaHistoriaSaludModel())];
    }
}
