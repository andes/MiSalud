using System.Text.Json.Serialization;

namespace AndesServices.DTOs.LaboratoriosRania;

public class InformeRaniaResponseDto
{
    [JsonPropertyName("protocolo_id")]
    public string ProtocoloId { get; set; }
    [JsonPropertyName("informe_url")]
    public string InformeUrl { get; set; }
}
