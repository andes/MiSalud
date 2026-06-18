using AndesServices.Interfaces;
using PacienteEntity = AndesServices.Entities.Paciente;

namespace SaludPortal.Application.UseCases.GrupoFamiliar;

public class ObtenerGrupoFamiliarUseCase
{
    private readonly IPaciente _pacienteService;

    public ObtenerGrupoFamiliarUseCase(IPaciente pacienteService)
    {
        _pacienteService = pacienteService;
    }

    public async Task<PacienteEntity?> EjecutarAsync(string pacienteId)
    {
        return await _pacienteService.ObtenerPacientePorIdAsync(pacienteId);
    }
}
