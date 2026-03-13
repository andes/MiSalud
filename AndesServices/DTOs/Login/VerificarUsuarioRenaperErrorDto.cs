using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login
{
    public class VerificarUsuarioRenaperErrorDto
    {
        [JsonPropertyName("resultado")]
        public string Resultado { get; set; } = string.Empty;

        [JsonPropertyName("mensaje")]
        public string? Mensaje { get; set; }
    }
}
