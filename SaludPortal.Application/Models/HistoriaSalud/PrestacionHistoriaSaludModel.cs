using System.Text.Json;

namespace SaludPortal.Application.Models.HistoriaSalud;

public class PrestacionHistoriaSaludModel
{
    public string? Id { get; set; }
    public SolicitudPrestacionModel? Solicitud { get; set; }
    public EjecucionPrestacionModel? Ejecucion { get; set; }
    public PacienteAdjuntosModel? Paciente { get; set; }

    public bool EsCda()
    {
        var conceptId = Solicitud?.TipoPrestacion?.ConceptId;
        return conceptId == "90226004" || conceptId == "86273004";
    }

    public bool EsEcocardiograma()
    {
        return Solicitud?.TipoPrestacion?.ConceptId == "5001000013101";
    }
}

public class ConceptoExtendidoModel
{
    public string? ConceptId { get; set; }
    public string? Term { get; set; }
    public string? Fsn { get; set; }
    public string? SemanticTag { get; set; }
    public string? Id { get; set; }
}

public class OrganizacionModel
{
    public string? Nombre { get; set; }
    public string? _id { get; set; }
}

public class ProfesionalModel
{
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Documento { get; set; }
}

public class RegistroModel
{
    public string? _id { get; set; }
    public string? Id { get; set; }
    public string? Nombre { get; set; }
    public ConceptoExtendidoModel? Concepto { get; set; }
    public JsonElement? Valor { get; set; }
    public bool? EsSolicitud { get; set; }
    public bool? EsDiagnosticoPrincipal { get; set; }
    public bool? Destacado { get; set; }
    public List<RegistroModel>? Registros { get; set; }
}

public class SolicitudPrestacionModel
{
    public ConceptoExtendidoModel? TipoPrestacion { get; set; }
    public OrganizacionModel? Organizacion { get; set; }
    public ProfesionalModel? Profesional { get; set; }
    public List<RegistroModel>? Registros { get; set; }
}

public class EjecucionPrestacionModel
{
    public DateTime? Fecha { get; set; }
}

public class PacienteAdjuntosModel
{
    public List<string>? Adjuntos { get; set; }
}
