using AndesServices.DTOs;

namespace AndesServices.Interfaces;

public interface IConsentimientoProgramaCuidadoIntegral
{
    Task<List<ConsentimientoDto>> ObtenerConsentimientosAsync(
        string pacienteId,
        string? programa = null,
        int? version = null,
        CancellationToken ct = default);

    Task<bool> ValidarPacienteAsync(string documento, string sexo, CancellationToken ct = default);

    Task<ConsentVersionDto?> ObtenerVersionProgramaAsync(string programa, CancellationToken ct = default);

    Task<ConsentimientoDto?> GuardarConsentimientoAsync(
        string programa,
        int version,
        string pacienteId,
        bool aceptacion,
        CancellationToken ct = default);
}
