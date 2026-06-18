using AndesServices.Interfaces;
using SaludPortal.Application.Models.Turnos;

namespace SaludPortal.Application.UseCases.Turnos;

public class SolicitarTeleconsultaUseCase
{
    private readonly IPaciente _pacienteService;
    private readonly ObtenerTurnosUseCase _obtenerTurnosUseCase;
    private readonly IMisTurnos _misTurnosService;

    public SolicitarTeleconsultaUseCase(IPaciente pacienteService, ObtenerTurnosUseCase obtenerTurnosUseCase, IMisTurnos misTurnosService)
    {
        _pacienteService = pacienteService;
        _obtenerTurnosUseCase = obtenerTurnosUseCase;
        _misTurnosService = misTurnosService;
    }

    public async Task<bool> EjecutarAsync(string pacienteId, TurnoDisponible t, string motivoConsulta, string telefono)
    {
        var paciente = await _pacienteService.ObtenerPacientePorIdAsync(pacienteId);
        if (paciente == null)
        {
            return false; // O manejar el caso de paciente no encontrado según sea necesario
        }

        // Validación defensiva: verificar que el paciente no tenga otro turno de teleconsulta
        var turnosPaciente = await _obtenerTurnosUseCase.EjecutarAsync();
        if (turnosPaciente != null && turnosPaciente.Any())
        {
            var tieneColision = turnosPaciente.Any(turno => turno.EsTeleconsulta);

            if (tieneColision)
            {
                return false; // Hay colisión con otro turno activo
            }
        }

        bool resultado = await _misTurnosService.RegistrarTurnoTeleConsultaAsync(
            t.Id,
            t.BloqueId,
            t.AgendaId,
            paciente,
            t.TipoPrestacionDetalle,
            motivoConsulta,
            telefono);
            
        return resultado;
    }
}
