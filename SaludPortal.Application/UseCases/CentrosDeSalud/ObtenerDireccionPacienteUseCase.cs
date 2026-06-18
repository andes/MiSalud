using AndesServices.Interfaces;
using SaludPortal.Application.Models.CentrosDeSalud;

namespace SaludPortal.Application.UseCases.CentrosDeSalud;

public class ObtenerDireccionPacienteUseCase
{
    private readonly IPaciente _pacienteService;

    public ObtenerDireccionPacienteUseCase(IPaciente pacienteService)
    {
        _pacienteService = pacienteService;
    }

    public async Task<DireccionPacienteResult?> EjecutarAsync(string pacienteId)
    {
        try
        {
            var paciente = await _pacienteService.ObtenerPacientePorIdAsync(pacienteId);
            var dir = _pacienteService.ObtenerDireccionPrioritaria(paciente);
            var geo = dir?.geoReferencia;

            if (geo == null || geo.Count < 2)
                return null;

            return new DireccionPacienteResult
            {
                Latitud = geo[0],
                Longitud = geo[1],
                Direccion = dir!.valor
            };
        }
        catch
        {
            return null;
        }
    }
}
