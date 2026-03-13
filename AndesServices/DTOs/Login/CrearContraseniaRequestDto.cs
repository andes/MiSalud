using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login;

public class CrearContraseniaRequestDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string CodigoActivacion { get; set; } = string.Empty;

    [JsonPropertyName("new_password")]
    public string NuevaContrasenia { get; set; } = string.Empty;
}
