using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AndesServices.DTOs.Turnos;

public class RegistrarTurnoRequestDto
{
    [JsonProperty("idAgenda")]
    [JsonPropertyName("idAgenda")]
    public string IdAgenda { get; set; }

    [JsonProperty("idBloque")]
    [JsonPropertyName("idBloque")]
    public string IdBloque { get; set; }

    [JsonProperty("idTurno")]
    [JsonPropertyName("idTurno")]
    public string IdTurno { get; set; }

    [JsonProperty("paciente")]
    [JsonPropertyName("paciente")]
    public RegistrarTurnoPacienteDto Paciente { get; set; }

    [JsonProperty("tipoPrestacion")]
    [JsonPropertyName("tipoPrestacion")]
    public RegistrarTurnoTipoPrestacionDto TipoPrestacion { get; set; }

    [JsonProperty("tipoTurno")]
    [JsonPropertyName("tipoTurno")]
    public string TipoTurno { get; set; }

    [JsonProperty("emitidoPor")]
    [JsonPropertyName("emitidoPor")]
    public string EmitidoPor { get; set; }

    [JsonProperty("nota")]
    [JsonPropertyName("nota")]
    public string Nota { get; set; }

    [JsonProperty("motivoConsulta")]
    [JsonPropertyName("motivoConsulta")]
    public string MotivoConsulta { get; set; }
}

public class RegistrarTurnoPacienteDto
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonProperty("documento")]
    [JsonPropertyName("documento")]
    public string? Documento { get; set; }

    [JsonProperty("nombre")]
    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonProperty("alias")]
    [JsonPropertyName("alias")]
    public string? Alias { get; set; }

    [JsonProperty("apellido")]
    [JsonPropertyName("apellido")]
    public string? Apellido { get; set; }

    [JsonProperty("fechaNacimiento")]
    [JsonPropertyName("fechaNacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [JsonProperty("telefono")]
    [JsonPropertyName("telefono")]
    public string? Telefono { get; set; }

    [JsonProperty("sexo")]
    [JsonPropertyName("sexo")]
    public string? Sexo { get; set; }

    [JsonProperty("obraSocial")]
    [JsonPropertyName("obraSocial")]
    public RegistrarTurnoObraSocialDto? ObraSocial { get; set; }
}

public class RegistrarTurnoObraSocialDto
{
    [JsonProperty("codigoPuco")]
    [JsonPropertyName("codigoPuco")]
    public int? CodigoPuco { get; set; }

    [JsonProperty("nombre")]
    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonProperty("financiador")]
    [JsonPropertyName("financiador")]
    public string? Financiador { get; set; }

    [JsonProperty("origen")]
    [JsonPropertyName("origen")]
    public string? Origen { get; set; }

    [JsonProperty("prepaga")]
    [JsonPropertyName("prepaga")]
    public bool? Prepaga { get; set; }
}

public class RegistrarTurnoTipoPrestacionDto
{
    [JsonProperty("auditable")]
    [JsonPropertyName("auditable")]
    public bool Auditable { get; set; }

    [JsonProperty("ambito")]
    [JsonPropertyName("ambito")]
    public List<string>? Ambito { get; set; }

    [JsonProperty("queries")]
    [JsonPropertyName("queries")]
    public List<object>? Queries { get; set; }

    [JsonProperty("_id")]
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonProperty("fsn")]
    [JsonPropertyName("fsn")]
    public string? Fsn { get; set; }

    [JsonProperty("semanticTag")]
    [JsonPropertyName("semanticTag")]
    public string? SemanticTag { get; set; }

    [JsonProperty("conceptId")]
    [JsonPropertyName("conceptId")]
    public string? ConceptId { get; set; }

    [JsonProperty("term")]
    [JsonPropertyName("term")]
    public string? Term { get; set; }

    [JsonProperty("multiprestacion")]
    [JsonPropertyName("multiprestacion")]
    public List<object>? Multiprestacion { get; set; }

    [JsonProperty("nombre")]
    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}
