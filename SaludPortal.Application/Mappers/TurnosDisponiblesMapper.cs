using AndesServices.Entities;
using SaludPortal.Application.Models.Turnos;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.Mappers;

public static class TurnosDisponiblesMapper
{
    public static List<TurnosPorTipoPrestacion> MapAgendaToTurnosPorTipoPrestacion(List<OrganizacionAgenda> organizacionAgendas, Dictionary<string, string> domiciliosOrganizaciones)
    {
        var tiposPrestaciones = ObtenerTipoPrestacionesDisponibles(organizacionAgendas);
        var turnosPorTipoPrestacion = new List<TurnosPorTipoPrestacion>();

        foreach (var (term, id) in tiposPrestaciones)
        {
            var efectores = ObtenerEfectores(organizacionAgendas, domiciliosOrganizaciones, id);
            turnosPorTipoPrestacion.Add(new TurnosPorTipoPrestacion
            {
                TipoPrestacionConceptId = id,
                TipoPrestacionTerm = term,
                Efectores = efectores.Select(e => new Efector
                {
                    Id = e.Efector._id,
                    Nombre = e.Efector.nombre,
                    Domicilio = e.Domicilio,
                    TurnosDisponibles = RecuperarAgendas(organizacionAgendas, id, e.Efector._id)
                        .SelectMany(item => (item.Bloque.turnos ?? new List<MisTurnos>())
                            .Where(t => t.estado == "disponible")
                            .Select(t => new TurnoDisponible
                            {
                                Id = t._id,
                                FechaHora = DateTimeHelper.ToArgentinaTime(t.horaInicio),
                                BloqueId = item.Bloque._id,
                                AgendaId = item.AgendaId,
                                TipoPrestacion = term,
                                NombreEfector = e.Efector.nombre,
                                DomicilioEfector = e.Domicilio,
                                TipoPrestacionDetalle = ObtenerTipoPrestacionPorId(organizacionAgendas, id)
                            })
                        )
                        .ToList()
                }).ToList()
            });
        }

        return turnosPorTipoPrestacion;
    }

    private static IEnumerable<(string term, string id)> ObtenerTipoPrestacionesDisponibles(List<OrganizacionAgenda> organizacionAgendas)
    {
        return organizacionAgendas
            .SelectMany(e => e.agendas)
            .Where(a => a.bloques != null && a.bloques.Count > 0 && a.bloques.Any(b => b.restantesMobile > 0))
            .SelectMany(a => a.tipoPrestaciones)
            .Where(tp => !string.IsNullOrWhiteSpace(tp.term))
            .Select(tp => (term: tp.term, id: tp._id))
            .Distinct()
            .OrderBy(t => t);
    }

    private static IEnumerable<(AgendaOrganizacion Efector, string Domicilio)> ObtenerEfectores(List<OrganizacionAgenda> organizacionAgendas, Dictionary<string, string> domiciliosOrganizaciones, string _idTipoPrestacion)
    {
        return organizacionAgendas?
            .SelectMany(e => e.agendas
                .Where(a => a.bloques != null && a.bloques.Any(b => b.restantesMobile > 0))
                .Where(a => (a.tipoPrestaciones ?? new List<AndesServices.Entities.TipoPrestacion>())
                    .Any(tp => tp!._id == _idTipoPrestacion))
                .Select(a => (
                    a.organizacion,
                    domiciliosOrganizaciones.TryGetValue(a.organizacion._id, out var domicilio) ? domicilio : ""
                ))
            )
            .Where(x => x.organizacion != null)
            .DistinctBy(x => x.organizacion!._id)
            ?? Enumerable.Empty<(AgendaOrganizacion, string)>();
    }

    private static IEnumerable<(string AgendaId, Bloque Bloque)> RecuperarAgendas(List<OrganizacionAgenda> organizacionAgendas, string _idTipoPrestacion, string Efector)
    {
        return organizacionAgendas
            .SelectMany(e => e.agendas
                    .Where(a =>
                a.bloques != null
                && a.organizacion?._id == Efector
                && (a.tipoPrestaciones ?? new List<AndesServices.Entities.TipoPrestacion>())
                    .Any(tp => tp._id == _idTipoPrestacion)
                && a.bloques.Any(b =>
                    b.restantesMobile > 0
                    && b.turnos.Any(c => c.estado == "disponible")
                )
            )
            .SelectMany(a => a.bloques
                .Where(b =>
                    b.restantesMobile > 0
                    && b.turnos.Any(c => c.estado == "disponible")
                )
                .Select(b => (a._id!, b))
            )
        );
    }

    private static AndesServices.Entities.TipoPrestacion? ObtenerTipoPrestacionPorId(List<OrganizacionAgenda> organizacionAgendas, string id)
    {
        return organizacionAgendas
            .SelectMany(e => e.agendas)
            .Where(a => a.bloques != null && a.bloques.Count > 0 && a.bloques.Any(b => b.restantesMobile > 0))
            .SelectMany(a => a.tipoPrestaciones)
            .FirstOrDefault(tp => tp._id == id);
    }
}
