using System.Text.Json;

namespace AndesServices.Entities
{
    public class CdaExtras
    {
        // Puede venir "id": "P-1462184" o "id": 47444 -> usar JsonElement para soportar ambos
        public JsonElement? id { get; set; }
        public string? organizacion { get; set; }
    }
}
