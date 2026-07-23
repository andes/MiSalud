using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.Turnos;
using SaludPortal.Application.UseCases.Paciente;

namespace SaludPortal.Application.UseCases.Turnos;

public class ObtenerTurnosDisponiblesUseCase
{
    private static readonly HashSet<string> EstadosActivos = ["programado", "asignado", "pendiente"];

    private readonly IMisTurnos _misTurnosService;
    private readonly IPaciente _pacienteService;
    private readonly IOrganizacion _organizacionService;
    private readonly ObtenerPacienteUseCase _obtenerPacienteUseCase;
    private readonly ObtenerTurnosUseCase _obtenerTurnosUseCase;

    public ObtenerTurnosDisponiblesUseCase(
        IMisTurnos misTurnosService,
        IPaciente pacienteService,
        IOrganizacion organizacionService,
        ObtenerPacienteUseCase obtenerPacienteUseCase,
        ObtenerTurnosUseCase obtenerTurnosUseCase
        )
    {
        _misTurnosService = misTurnosService;
        _pacienteService = pacienteService;
        _organizacionService = organizacionService;
        _obtenerPacienteUseCase = obtenerPacienteUseCase;
        _obtenerTurnosUseCase = obtenerTurnosUseCase;
    }

    public async Task<List<TurnosPorTipoPrestacion>> EjecutarAsync(string idPaciente, bool esTeleconsulta = false)
    {
        var paciente = await _obtenerPacienteUseCase.EjecutarAsync(idPaciente);
        var turnosPaciente = await _obtenerTurnosUseCase.EjecutarAsync();

        var direccionGeografica = paciente?.ObtenerDireccionPrioritaria();

        if (direccionGeografica == null)
        {
            return [];
        }

        if (direccionGeografica.Valor == null || direccionGeografica?.Ubicacion?.Localidad == null || direccionGeografica?.Ubicacion?.Provincia == null)
        {
            return [];
        }

        string direccion = direccionGeografica.Valor + ", " + direccionGeografica.Ubicacion.Localidad.Nombre + ", " + direccionGeografica.Ubicacion.Provincia.Nombre;
        var userLocation = await _pacienteService.ObtenerGeoreferenciaPaciente(direccion);

        if (userLocation == null)
        {
            return [];
        }

        var agendasOrganizacionales = await _misTurnosService.ObtenerAgendasOrganizaciones(idPaciente, userLocation, esTeleconsulta);

        Dictionary<string, string> domiciliosOrganizaciones = new Dictionary<string, string>();

        // Obtener todos los IDs de organizaciones únicos
        var orgIds = agendasOrganizacionales
            .SelectMany(e => e.agendas)
            .Select(a => a.organizacion._id)
            .Distinct()
            .ToList();

        // Llamar a ObtenerOrganizacionPorIdAsync para cada organización y guardar el domicilio
        foreach (var orgId in orgIds)
        {
            var organizacion = await _organizacionService.ObtenerOrganizacionPorIdAsync(orgId);
            if (organizacion != null && !domiciliosOrganizaciones.ContainsKey(orgId))
            {
                domiciliosOrganizaciones[orgId] = organizacion.direccion?.valor ?? "";
            }
        }

        if (agendasOrganizacionales == null || agendasOrganizacionales.Count == 0)
        {
            return [];
        }

        var todosLosTurnos = TurnosDisponiblesMapper.MapAgendaToTurnosPorTipoPrestacion(agendasOrganizacionales, domiciliosOrganizaciones);

        return FiltrarPorTurnosPaciente(todosLosTurnos, turnosPaciente);
    }

    private static List<TurnosPorTipoPrestacion> FiltrarPorTurnosPaciente(
        List<TurnosPorTipoPrestacion> turnosDisponibles,
        List<Turno> turnosPaciente)
    {
        var turnosActivos = turnosPaciente
            .Where(t => t.Estado != null && EstadosActivos.Contains(t.Estado))
            .ToList();

        var conceptIdsConTurnoActivo = turnosActivos
            .Where(t => t.TipoPrestacion?.ConceptId != null)
            .Select(t => t.TipoPrestacion!.ConceptId!)
            .ToHashSet();

        // Construir HashSet con todas las fechas ocupadas (solo turnos activos)
        var fechasOcupadas = turnosActivos
            .Select(t => t.FechaHora)
            .ToHashSet();

        foreach (var grupo in turnosDisponibles)
        {
            foreach (var efector in grupo.Efectores)
            {
                efector.TurnosDisponibles = efector.TurnosDisponibles
                    .Where(t =>
                    {
                        // Filtrar por colisión de horario (cualquier turno del paciente, sin importar estado)
                        if (fechasOcupadas.Contains(t.FechaHora))
                            return false;

                        var conceptId = t.TipoPrestacionDetalle?.conceptId;
                        if (conceptId != null && conceptIdsConTurnoActivo.Contains(conceptId))
                            return false;

                        return true;
                    })
                    .ToList();
            }

            grupo.Efectores = grupo.Efectores
                .Where(e => e.TurnosDisponibles.Count > 0)
                .ToList();
        }

        return turnosDisponibles
            .Where(g => g.Efectores.Count > 0)
            .ToList();
    }
}
