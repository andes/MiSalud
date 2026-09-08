using System;
using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.Turnos;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.UseCases.Turnos;

public class ObtenerTurnosUseCase
{
    private readonly IMisTurnos _misTurnosService;
    private readonly TurnosConfiguracion _turnosConfig;

    public ObtenerTurnosUseCase(IMisTurnos misTurnosService, TurnosConfiguracion turnosConfig)
    {
        _misTurnosService = misTurnosService;
        _turnosConfig = turnosConfig;
    }

    public async Task<List<Turno>> EjecutarAsync()
    {
        var conceptosTeleconsulta = new HashSet<string>();
        var conceptosTurneablesList = await _misTurnosService.ObtenerConceptosTurneablesAsync(true);
        if (conceptosTurneablesList != null && conceptosTurneablesList.Count > 0)
        {
            conceptosTeleconsulta = [.. conceptosTurneablesList.Select(c => c.conceptId)];
        }

        var ahora = DateTimeHelper.ToArgentinaTime(DateTime.UtcNow);
        var minutosVisualizacion = Math.Max(0, _turnosConfig.MinutosVisualizacionTelemedicina);

        // Obtengo los turnos
        return (await _misTurnosService.ObtenerMisTurnosAsync(""))
            .Where(t => t != null)
            .Select(t =>
            {
                var turno = t.MapToTurno(conceptosTeleconsulta);
                if (turno.VideoConferencia)
                {
                    var finVentana = turno.FechaHora.AddMinutes(minutosVisualizacion);
                    turno.PuedeIngresarVideollamada =
                        turno.EsDiaDelTurno
                        || (ahora >= turno.FechaHora && ahora < finVentana);
                }
                return turno;
            })
            .Where(t => t.FechaHora >= ahora
                || (t.VideoConferencia && t.FechaHora.AddMinutes(minutosVisualizacion) >= ahora))
            .OrderBy(t => t.FechaHora)
            .ToList();
    }
}
