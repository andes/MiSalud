using System.Text.Json.Serialization;

namespace AndesServices.DTOs;

public class ErrorDto
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
