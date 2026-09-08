using System;
using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.Turnos;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.UseCases.Turnos;

public class ObtenerHistorialTurnosUseCase
{
    private readonly IMisTurnos _misTurnosService;
    private readonly TurnosConfiguracion _turnosConfig;

    public ObtenerHistorialTurnosUseCase(IMisTurnos misTurnosService, TurnosConfiguracion turnosConfig)
    {
        _misTurnosService = misTurnosService;
        _turnosConfig = turnosConfig;
    }

    public async Task<List<Turno>> EjecutarAsync()
    {
        var ahora = DateTimeHelper.ToArgentinaTime(DateTime.UtcNow);
        var minutosVisualizacion = Math.Max(0, _turnosConfig.MinutosVisualizacionTelemedicina);

        var turnos = await _misTurnosService.ObtenerMisTurnosAsync("");
        return turnos
            .Where(t => t != null)
            .Select(t => t.MapToTurno())
            .Where(t => EsTurnoPasado(t, ahora, minutosVisualizacion))
            .OrderByDescending(t => t.FechaHora)
            .ToList();
    }

    /// <summary>
    /// Un turno está en historial si ya pasó su hora de inicio.
    /// Los de videoconferencia permanecen en Mis Turnos durante la ventana
    /// configurable y solo entran al historial al cerrarse esa ventana.
    /// </summary>
    private static bool EsTurnoPasado(Turno turno, DateTime ahora, int minutosVisualizacion)
    {
        if (turno.VideoConferencia)
            return turno.FechaHora.AddMinutes(minutosVisualizacion) < ahora;

        return turno.FechaHora < ahora;
    }
}
