using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login;

public class OlvideContraseniaRequestDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("origen")]
    public string Origen { get; set; }
}
