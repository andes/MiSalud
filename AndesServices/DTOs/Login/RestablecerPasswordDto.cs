using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login;

public class RestablecerPasswordDto
{
    [JsonPropertyName("restablecerPassword")]
    public bool RestablecerPassword { get; set; }
}
