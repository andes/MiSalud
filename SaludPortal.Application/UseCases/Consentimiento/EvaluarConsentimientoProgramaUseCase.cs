using AndesServices.Interfaces;
using SaludPortal.Application.Models.Consentimiento;

namespace SaludPortal.Application.UseCases.Consentimiento;

public class EvaluarConsentimientoProgramaUseCase
{
    private readonly IConsentimientoProgramaCuidadoIntegral _consentimientoService;

    public EvaluarConsentimientoProgramaUseCase(IConsentimientoProgramaCuidadoIntegral consentimientoService)
    {
        _consentimientoService = consentimientoService;
    }

    public async Task<EvaluarConsentimientoResult> EjecutarAsync(
        string pacienteId,
        string documento,
        string sexo,
        DateTime? fechaNacimiento)
    {
        var resultado = new EvaluarConsentimientoResult();

        if (string.IsNullOrWhiteSpace(pacienteId) ||
            string.IsNullOrWhiteSpace(documento) ||
            string.IsNullOrWhiteSpace(sexo) ||
            !fechaNacimiento.HasValue)
        {
            return resultado;
        }

        if (CalcularEdad(fechaNacimiento.Value) < ConsentimientoConstants.EdadMinimaPrograma)
        {
            return resultado;
        }

        var consentimientos = await _consentimientoService.ObtenerConsentimientosAsync(
            pacienteId,
            ConsentimientoConstants.ProgramaCuidar65);
        if (consentimientos.Count > 0)
        {
            return resultado;
        }

        var validado = await _consentimientoService.ValidarPacienteAsync(documento, sexo);
        if (!validado)
        {
            return resultado;
        }

        var version = await _consentimientoService.ObtenerVersionProgramaAsync(ConsentimientoConstants.ProgramaCuidar65);
        if (version is null)
        {
            return resultado;
        }

        resultado.MostrarPopup = true;
        resultado.Version = ConsentimientoMapper.ToModel(version);
        return resultado;
    }

    private static int CalcularEdad(DateTime fechaNacimiento)
    {
        var hoy = DateTime.Today;
        var edad = hoy.Year - fechaNacimiento.Year;
        if (fechaNacimiento.Date > hoy.AddYears(-edad))
        {
            edad--;
        }

        return edad;
    }
}
