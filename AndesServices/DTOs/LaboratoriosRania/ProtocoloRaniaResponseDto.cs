using System.Text.Json.Serialization;

namespace AndesServices.DTOs.LaboratoriosRania;

public class ProtocoloRaniaResponseDto
{
    [JsonPropertyName("protocolo_id")]
    public string ProtocoloId { get; set; }

    [JsonPropertyName("protocolo")]
    public string Protocolo { get; set; }

    [JsonPropertyName("fecha")]
    public string Fecha { get; set; }

    [JsonPropertyName("solicitante_apellido")]
    public string SolicitanteApellido { get; set; }

    [JsonPropertyName("solicitante_nombre")]
    public string SolicitanteNombre { get; set; }

    [JsonPropertyName("matricula")]
    public string Matricula { get; set; }

    [JsonPropertyName("profesion")]
    public string Profesion { get; set; }
}
