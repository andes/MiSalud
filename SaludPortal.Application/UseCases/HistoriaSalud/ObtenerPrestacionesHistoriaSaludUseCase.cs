using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.HistoriaSalud;

namespace SaludPortal.Application.UseCases.HistoriaSalud;

public class ObtenerPrestacionesHistoriaSaludUseCase
{
    private readonly IHistoriaSalud _historiaSaludService;

    public ObtenerPrestacionesHistoriaSaludUseCase(IHistoriaSalud historiaSaludService)
    {
        _historiaSaludService = historiaSaludService;
    }

    public async Task<List<PrestacionHistoriaSaludModel>> EjecutarAsync(string expresionSnomed, string pacienteId)
    {
        var prestaciones = await _historiaSaludService.ObtenerPrestacionesAsync(expresionSnomed, pacienteId);
        if (prestaciones == null) return [];
        return [.. prestaciones.Select(p => p.MapToPrestacionHistoriaSaludModel())];
    }
}
