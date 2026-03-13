using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login;

public class ValidarCodigoActivacionResponseDto
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonProperty("error")]
    public string? Error { get; set; }
}
