using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login;

public class RegistroResponseDto
{
    [JsonPropertyName("activacionApp")]
    public bool ActivacionApp { get; set; }

    [JsonPropertyName("permisos")]
    public List<object> Permisos { get; set; } = [];

    [JsonPropertyName("_id")]
    public string IdInterno { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("documento")]
    public string Documento { get; set; } = string.Empty;

    [JsonPropertyName("sexo")]
    public string Sexo { get; set; } = string.Empty;

    [JsonPropertyName("telefono")]
    public string Telefono { get; set; } = string.Empty;

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [JsonPropertyName("apellido")]
    public string Apellido { get; set; } = string.Empty;

    [JsonPropertyName("fechaNacimiento")]
    public DateTime FechaNacimiento { get; set; }

    [JsonPropertyName("pacientes")]
    public List<RegistroPacienteDto> Pacientes { get; set; } = [];

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("devices")]
    public List<object> Devices { get; set; } = [];

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public class RegistroPacienteDto
{
    [JsonPropertyName("relacion")]
    public string Relacion { get; set; } = string.Empty;

    [JsonPropertyName("_id")]
    public string IdInterno { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("addedAt")]
    public DateTime AddedAt { get; set; }
}
