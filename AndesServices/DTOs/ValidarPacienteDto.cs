using System.Text.Json.Serialization;

namespace AndesServices.DTOs;

public class ValidarPacienteRequest
{
    [JsonPropertyName("documento")]
    public string Documento { get; set; } = string.Empty;

    [JsonPropertyName("sexo")]
    public string Sexo { get; set; } = string.Empty;
}

public class ValidarPacienteResponse
{
    [JsonPropertyName("validado")]
    public bool Validado { get; set; }

    [JsonPropertyName("mensaje")]
    public string? Mensaje { get; set; }
}
