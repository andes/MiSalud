using AndesServices.Interfaces;

namespace SaludPortal.Application.UseCases.Turnos;

public class CancelarTurnoUseCase
{
    private readonly IPaciente _pacienteService;
    private readonly IMisTurnos _misTurnosService;

    public CancelarTurnoUseCase(IPaciente pacienteService, IMisTurnos misTurnosService)
    {
        _pacienteService = pacienteService;
        _misTurnosService = misTurnosService;
    }

    public async Task<bool> EjecutarAsync(string pacienteId, string idTurno, string idBloque, string idAgenda)
    {
        var paciente = await _pacienteService.ObtenerPacientePorIdAsync(pacienteId);
        if (paciente == null)
            return false;

        return await _misTurnosService.CancelarTurnoAsync(idTurno, idBloque, idAgenda, paciente);
    }
}
