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

        // Obtengo los turnos
        return (await _misTurnosService.ObtenerMisTurnosAsync(""))
            .Where(t => t != null && t.horaInicio >= DateTime.Now)
            .OrderBy(t => t.horaInicio)
            .Select(t => t.MapToTurno(conceptosTeleconsulta))
            .ToList();
    }
}
