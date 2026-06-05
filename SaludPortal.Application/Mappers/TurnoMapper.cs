using System;
using AndesServices.Entities;
using SaludPortal.Application.Models;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.Mappers;

public static class TurnoMapper
{
    public static Turno MapToTurno(this MisTurnos t, HashSet<string>? conceptosTeleconsulta = null)
    {
        return new Turno
        {
            Id = t._id,
            EsTeleconsulta = EsTeleconsulta(conceptosTeleconsulta, t.tipoPrestacion?.conceptId),
            Paciente = t.paciente == null ? null : new Models.Paciente
            {
                Nombre = t.paciente.nombre,
                Apellido = t.paciente.apellido,
                Documento = t.paciente.documento,
                Alias = t.paciente.alias,
            },
            TipoPrestacion = t.tipoPrestacion == null ? null : new Models.TipoPrestacion
            {
                ConceptId = t.tipoPrestacion.conceptId,
                Term = t.tipoPrestacion.term
            },
            Organizacion = t.organizacion == null ? null : new Models.Organizacion
            {
                Id = t.organizacion._id,
                Nombre = t.organizacion.nombre
            },
            FechaHora = DateTimeHelper.ToArgentinaTime(t.horaInicio),
            DuracionMinutos = t.duracionTurno,
            Asistencia = t.asistencia,
            FechaHoraAsistencia = t.horaAsistencia.HasValue ? DateTimeHelper.ToArgentinaTime(t.horaAsistencia.Value) : (DateTime?)null,
            AgendaId = t.agenda_id,
            BloqueId = t.bloque_id,
            MotivoConsulta = t.motivoConsulta,
            Profesionales = t.profesionales?.Select(p => new Models.Profesional
            {
                Id = p._id,
                Nombre = p.nombre,
                Apellido = p.apellido,
            }).ToList(),
            VideoConferencia = t?.videoConferencia ?? false,
            WebexLinks = t?.webexLinks == null ? null : new Models.WebexLinks
            {
                professionalLink = t.webexLinks.professionalLink,
                patientLink = t.webexLinks.patientLink
            }
        };
    }

    private static bool EsTeleconsulta(HashSet<string>? conceptosTeleconsulta, string? conceptId)
    {
        if (conceptosTeleconsulta == null || string.IsNullOrEmpty(conceptId))
            return false;
        return conceptosTeleconsulta.Contains(conceptId);
    }
}
