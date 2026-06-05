using System;
using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models;

namespace SaludPortal.Application.UseCases;

public class ObtenerTurnosUseCase
{
    private readonly IMisTurnos _misTurnosService;

    public ObtenerTurnosUseCase(IMisTurnos misTurnosService)
    {
        _misTurnosService = misTurnosService;
    }

    public async Task<List<Turno>> EjecutarAsync()
    {
        var conceptosTeleconsulta = new HashSet<string>();
        var conceptosTurneablesList = await _misTurnosService.ObtenerConceptosTurneablesAsync(true);
        if (conceptosTurneablesList != null && conceptosTurneablesList.Count > 0)
        {
            conceptosTeleconsulta = [.. conceptosTurneablesList.Select(c => c.conceptId)];
        }

        var ahora = DateTime.Now;

        // Obtengo los turnos
        return (await _misTurnosService.ObtenerMisTurnosAsync(""))
            .Where(t => t != null)
            .Select(t => t.MapToTurno(conceptosTeleconsulta))
            .Where(t => t.FechaHora >= ahora || (t.VideoConferencia && t.FechaHora.AddHours(1) >= ahora))
            .OrderBy(t => t.FechaHora)
            .ToList();
    }
}
