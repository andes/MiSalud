using System.Text.Json.Serialization;

namespace AndesServices.DTOs;

public class ConsentVersionDto
{
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("programa")]
    public string? Programa { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("titulo")]
    public string? Titulo { get; set; }

    [JsonPropertyName("texto")]
    public string? Texto { get; set; }

    [JsonPropertyName("formato")]
    public string? Formato { get; set; }

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}
