using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.Farmacias;

namespace SaludPortal.Application.UseCases.Farmacias;

public class ObtenerFarmaciasTurnoUseCase
{
    private readonly IFarmaciasTurno _farmaciasTurnoService;

    public ObtenerFarmaciasTurnoUseCase(IFarmaciasTurno farmaciasTurnoService)
    {
        _farmaciasTurnoService = farmaciasTurnoService;
    }

    public async Task<List<Farmacia>> EjecutarAsync(string localidadId)
    {
        var hoy = DateTime.Now.ToString("yyyy-MM-dd");
        var farmacias = await _farmaciasTurnoService.ObtenerFarmaciasTurnoAsync(localidadId, hoy, hoy);
        return farmacias?
            .Select(f => f.MapToFarmacia())
            .ToList() ?? [];
    }
}

