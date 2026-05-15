using System;
using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models;

namespace SaludPortal.Application.UseCases;

public class ObtenerHistorialTurnosUseCase
{
    private readonly IMisTurnos _misTurnosService;

    public ObtenerHistorialTurnosUseCase(IMisTurnos misTurnosService)
    {
        _misTurnosService = misTurnosService;
    }

    public async Task<List<Turno>> EjecutarAsync()
    {
        // Obtengo los turnos
        var turnos = await _misTurnosService.ObtenerMisTurnosAsync("");
        // Mapear a Turno y ordenar por fecha de inicio descendente
        return turnos
            .OrderByDescending(t => t.horaInicio)
            .Select(t => t.MapToTurno())
            .ToList();
    }
}
