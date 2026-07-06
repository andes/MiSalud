using System.Text.Json.Serialization;

namespace AndesServices.DTOs;

public class ConsentimientoDto
{
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("programa")]
    public string? Programa { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("pacienteId")]
    public string? PacienteId { get; set; }

    [JsonPropertyName("aceptacion")]
    public bool Aceptacion { get; set; }

    [JsonPropertyName("fechaResp")]
    public DateTime FechaResp { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}
