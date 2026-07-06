using AndesServices.Interfaces;
using SaludPortal.Application.Models.Consentimiento;

namespace SaludPortal.Application.UseCases.Consentimiento;

public class ObtenerConsentimientosUseCase
{
    private readonly IConsentimientoProgramaCuidadoIntegral _consentimientoService;

    public ObtenerConsentimientosUseCase(IConsentimientoProgramaCuidadoIntegral consentimientoService)
    {
        _consentimientoService = consentimientoService;
    }

    public async Task<List<ConsentimientoModel>> EjecutarAsync(string pacienteId)
    {
        if (string.IsNullOrWhiteSpace(pacienteId))
        {
            return [];
        }

        var consentimientos = await _consentimientoService.ObtenerConsentimientosAsync(pacienteId);
        return ConsentimientoMapper.UltimoEstadoPorPrograma(consentimientos);
    }
}
