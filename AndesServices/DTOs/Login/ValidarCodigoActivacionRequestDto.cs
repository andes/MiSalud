using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login;

public class ValidarCodigoActivacionRequestDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    public string CodigoActivacion { get; set; }
}
