using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login;

public class ReestablecerPasswordRequestDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("codigo")]
    public string Codigo { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("password2")]
    public string Password2 { get; set; }
}
