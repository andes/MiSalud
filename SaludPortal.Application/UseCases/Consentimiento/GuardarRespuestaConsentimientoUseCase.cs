using AndesServices.Interfaces;
using SaludPortal.Application.Models.Consentimiento;

namespace SaludPortal.Application.UseCases.Consentimiento;

public class GuardarRespuestaConsentimientoUseCase
{
    private readonly IConsentimientoProgramaCuidadoIntegral _consentimientoService;

    public GuardarRespuestaConsentimientoUseCase(IConsentimientoProgramaCuidadoIntegral consentimientoService)
    {
        _consentimientoService = consentimientoService;
    }

    public async Task<ConsentimientoModel?> EjecutarAsync(
        string programa,
        int version,
        string pacienteId,
        bool aceptacion)
    {
        if (string.IsNullOrWhiteSpace(programa) ||
            string.IsNullOrWhiteSpace(pacienteId) ||
            version <= 0)
        {
            return null;
        }

        var resultado = await _consentimientoService.GuardarConsentimientoAsync(
            programa,
            version,
            pacienteId,
            aceptacion);

        return resultado is null ? null : ConsentimientoMapper.ToModel(resultado);
    }
}
