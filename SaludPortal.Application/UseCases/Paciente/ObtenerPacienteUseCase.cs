using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.Paciente;

namespace SaludPortal.Application.UseCases.Paciente;

public class ObtenerPacienteUseCase
{
    private readonly IPaciente _pacienteService;

    public ObtenerPacienteUseCase(IPaciente pacienteService)
    {
        _pacienteService = pacienteService;
    }

    public async Task<PacienteModel?> EjecutarAsync(string idPaciente)
    {
        var paciente = await _pacienteService.ObtenerPacientePorIdAsync(idPaciente);

        if (paciente == null)
        {
            return null;
        }

        return paciente.MapToPacienteModel();
    }

    public async Task<PacienteModel?> EjecutarAsync(string idPaciente, string token)
    {
        var paciente = await _pacienteService.ObtenerPacientePorIdAsync(idPaciente, token);

        if (paciente == null)
        {
            return null;
        }

        return paciente.MapToPacienteModel();
    }
}
