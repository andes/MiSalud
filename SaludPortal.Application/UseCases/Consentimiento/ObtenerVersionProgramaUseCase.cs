using AndesServices.Interfaces;
using SaludPortal.Application.Models.Consentimiento;

namespace SaludPortal.Application.UseCases.Consentimiento;

public class ObtenerVersionProgramaUseCase
{
    private readonly IConsentimientoProgramaCuidadoIntegral _consentimientoService;

    public ObtenerVersionProgramaUseCase(IConsentimientoProgramaCuidadoIntegral consentimientoService)
    {
        _consentimientoService = consentimientoService;
    }

    public async Task<ConsentVersionModel?> EjecutarAsync(string programa)
    {
        if (string.IsNullOrWhiteSpace(programa))
        {
            return null;
        }

        var version = await _consentimientoService.ObtenerVersionProgramaAsync(programa);
        return version is null ? null : ConsentimientoMapper.ToModel(version);
    }
}
