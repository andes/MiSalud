using AndesServices.Entities;
using SaludPortal.Application.Models.HistoriaSalud;

namespace SaludPortal.Application.Mappers;

public static class HistoriaSaludMapper
{
    public static CategoriaHistoriaSaludModel MapToCategoriaHistoriaSaludModel(this CategoriaHistoriaSalud c)
    {
        return new CategoriaHistoriaSaludModel
        {
            Id = c.Id ?? c._id,
            Titulo = c.Titulo,
            ExpresionSnomed = c.ExpresionSnomed,
            BusquedaPor = c.BusquedaPor,
            DescargaAdjuntos = c.DescargaAdjuntos
        };
    }

    public static PrestacionHistoriaSaludModel MapToPrestacionHistoriaSaludModel(this PrestacionHistoriaSalud p)
    {
        return new PrestacionHistoriaSaludModel
        {
            Id = p.id,
            Solicitud = p.solicitud?.MapToSolicitudPrestacionModel(),
            Ejecucion = p.ejecucion?.MapToEjecucionPrestacionModel(),
            Paciente = p.paciente?.MapToPacienteAdjuntosModel()
        };
    }

    private static SolicitudPrestacionModel MapToSolicitudPrestacionModel(this SolicitudPrestacion s)
    {
        return new SolicitudPrestacionModel
        {
            TipoPrestacion = s.tipoPrestacion?.MapToConceptoExtendidoModel(),
            Organizacion = s.organizacion?.MapToOrganizacionModel(),
            Profesional = s.profesional?.MapToProfesionalModel(),
            Registros = s.registros?.Select(r => r.MapToRegistroModel()).ToList()
        };
    }

    private static RegistroModel MapToRegistroModel(this Registro r)
    {
        return new RegistroModel
        {
            _id = r._id,
            Id = r.id,
            Nombre = r.nombre,
            Concepto = r.concepto?.MapToConceptoExtendidoModel(),
            Valor = r.valor,
            EsSolicitud = r.esSolicitud,
            EsDiagnosticoPrincipal = r.esDiagnosticoPrincipal,
            Destacado = r.destacado,
            Registros = r.registros?.Select(inner => inner.MapToRegistroModel()).ToList()
        };
    }

    private static EjecucionPrestacionModel MapToEjecucionPrestacionModel(this EjecucionPrestacion e)
    {
        return new EjecucionPrestacionModel
        {
            Fecha = e.fecha
        };
    }

    private static ConceptoExtendidoModel MapToConceptoExtendidoModel(this ConceptoExtendido c)
    {
        return new ConceptoExtendidoModel
        {
            ConceptId = c.conceptId,
            Term = c.term,
            Fsn = c.fsn,
            SemanticTag = c.semanticTag,
            Id = c.id
        };
    }

    private static OrganizacionModel MapToOrganizacionModel(this Organizacion o)
    {
        return new OrganizacionModel
        {
            Nombre = o.nombre,
            _id = o._id
        };
    }

    private static ProfesionalModel MapToProfesionalModel(this Profesional p)
    {
        return new ProfesionalModel
        {
            Nombre = p.nombre,
            Apellido = p.apellido,
            Documento = p.documento
        };
    }

    private static PacienteAdjuntosModel MapToPacienteAdjuntosModel(this Paciente p)
    {
        return new PacienteAdjuntosModel
        {
            Adjuntos = p.adjuntos
        };
    }
}
