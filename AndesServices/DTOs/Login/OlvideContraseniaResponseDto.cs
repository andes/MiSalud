using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login;

public class OlvideContraseniaResponseDto
{
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}
