using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.CentrosDeSalud;

namespace SaludPortal.Application.UseCases.CentrosDeSalud;

public class ObtenerCentrosSaludUseCase
{
    private readonly ICentrosSalud _centrosSaludService;

    public ObtenerCentrosSaludUseCase(ICentrosSalud centrosSaludService)
    {
        _centrosSaludService = centrosSaludService;
    }

    public async Task<List<CentroSaludModel>> EjecutarAsync(string origen)
    {
        if (origen == "provincia")
        {
            var centros = await _centrosSaludService.ObtenerCentrosDeSaludProvincia();
            return centros.Select(c => c.MapToCentroSaludModel()).ToList();
        }
        else
        {
            var centros = await _centrosSaludService.ObtenerCentrosDeSaludAraucania();
            return centros.Select(c => c.MapToCentroSaludModel()).ToList();
        }
    }
}
