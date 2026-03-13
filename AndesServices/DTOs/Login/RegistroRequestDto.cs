using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login;

public class RegistroRequestDto
{
    [JsonPropertyName("scanText")]
    public string ScanText { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("documento")]
    public string Documento { get; set; }

    [JsonPropertyName("sexo")]
    public string Sexo { get; set; }

    [JsonPropertyName("telefono")]
    public string Telefono { get; set; }

    [JsonPropertyName("recaptcha")]
    public string Recaptcha { get; set; } = string.Empty;

    [JsonPropertyName("scan")]
    public bool Scan { get; set; } = true;
}
