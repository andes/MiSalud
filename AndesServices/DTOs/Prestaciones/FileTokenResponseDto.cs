using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Prestaciones
{
    public class FileTokenResponseDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = "";
    }
}
