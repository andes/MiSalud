using AndesServices.Interfaces;

namespace SaludPortal.Application.UseCases.HistoriaSalud;

public class ObtenerUrlImagenPrestacionUseCase
{
    private readonly IHistoriaSalud _historiaSaludService;

    public ObtenerUrlImagenPrestacionUseCase(IHistoriaSalud historiaSaludService)
    {
        _historiaSaludService = historiaSaludService;
    }

    public async Task<string?> EjecutarAsync(string idPrestacion, string fileToken)
    {
        return await _historiaSaludService.ObtenerImagenPrestacionUrlAsync(idPrestacion, fileToken);
    }
}
