using System.Text.Json.Serialization;
using AndesServices.Entities;

namespace AndesServices.DTOs.Login;

public class CrearContraseniaResponseDto
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("user")]
    public User? User { get; set; }
}
