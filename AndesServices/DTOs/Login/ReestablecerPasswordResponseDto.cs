using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login;

public class ReestablecerPasswordResponseDto
{
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}
