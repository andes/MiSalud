using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.Farmacias;

namespace SaludPortal.Application.UseCases.Farmacias;

public class ObtenerLocalidadesDeFarmaciasUseCase
{
    private readonly IFarmaciasTurno _farmaciasTurnoService;

    public ObtenerLocalidadesDeFarmaciasUseCase(IFarmaciasTurno farmaciasTurnoService)
    {
        _farmaciasTurnoService = farmaciasTurnoService;
    }

    public async Task<List<LocalidadModel>> EjecutarAsync()
    {
        // Obtengo las localidades
        var localidades = await _farmaciasTurnoService.ObtenerLocalidadesAsync();

        if (localidades == null)
        {
            return [];
        }

        // Mapeo a LocalidadModel
        return [.. localidades.Select(loc => loc.MapToLocalidadModel())];
    }
}
