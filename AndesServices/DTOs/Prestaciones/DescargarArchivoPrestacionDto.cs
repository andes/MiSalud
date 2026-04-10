using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Prestaciones
{
    public class DescargarArchivoPrestacionDto
    {
        [JsonPropertyName("idPrestacion")]
        public string IdPrestacion { get; set; } = string.Empty;
    }
}