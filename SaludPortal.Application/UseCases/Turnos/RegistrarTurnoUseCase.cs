using AndesServices.Interfaces;
using SaludPortal.Application.Models.Turnos;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.UseCases.Turnos;

public class RegistrarTurnoUseCase
{
    private readonly IPaciente _pacienteService;
    private readonly ObtenerTurnosUseCase _obtenerTurnosUseCase;
    private readonly IMisTurnos _misTurnosService;

    public RegistrarTurnoUseCase(IPaciente pacienteService, ObtenerTurnosUseCase obtenerTurnosUseCase, IMisTurnos misTurnosService)
    {
        _pacienteService = pacienteService;
        _obtenerTurnosUseCase = obtenerTurnosUseCase;
        _misTurnosService = misTurnosService;
    }

    public async Task<bool> EjecutarAsync(string pacienteId, TurnoDisponible t)
    {
        var paciente = await _pacienteService.ObtenerPacientePorIdAsync(pacienteId);
        if (paciente == null)
        {
            return false; // O manejar el caso de paciente no encontrado según sea necesario
        }

        // Validación defensiva: verificar que el paciente no tenga otro turno activo en el mismo horario
        var turnosPaciente = await _obtenerTurnosUseCase.EjecutarAsync();
        if (turnosPaciente != null && turnosPaciente.Any())
        {
            var tieneColision = turnosPaciente.Any(turno => turno.FechaHora == t.FechaHora);

            if (tieneColision)
            {
                return false; // Hay colisión de horario con otro turno activo
            }
        }

        var tipoTurno = t.FechaHora.Date == DateTimeHelper.NowArgentina().Date
            ? "delDia"
            : "programado";

        bool resultado = await _misTurnosService.RegistrarTurnoAsync(t.Id, t.BloqueId, t.AgendaId, paciente, t.TipoPrestacionDetalle, tipoTurno);
        return resultado;
    }
}
