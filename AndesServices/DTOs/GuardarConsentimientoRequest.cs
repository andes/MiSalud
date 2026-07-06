using System.Text.Json.Serialization;

namespace AndesServices.DTOs;

public class GuardarConsentimientoRequest
{
    [JsonPropertyName("programa")]
    public string Programa { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("pacienteId")]
    public string PacienteId { get; set; } = string.Empty;

    [JsonPropertyName("aceptacion")]
    public bool Aceptacion { get; set; }
}
