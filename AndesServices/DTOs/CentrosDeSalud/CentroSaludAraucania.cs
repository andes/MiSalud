using Newtonsoft.Json;

namespace AndesServices.DTOs.CentrosDeSalud;

public partial class CentroSaludAraucania
{
    [JsonProperty("_id")]
    public string Id { get; set; }

    [JsonProperty("longitud")]
    public double Longitud { get; set; }

    [JsonProperty("latitud")]
    public double Latitud { get; set; }

    [JsonProperty("nombre")]
    public string Nombre { get; set; }

    [JsonProperty("region")]
    public string Region { get; set; }

    [JsonProperty("comunidad")]
    public string Comunidad { get; set; }

    [JsonProperty("complejidad")]
    public string Complejidad { get; set; }

    [JsonProperty("direccion")]
    public string Direccion { get; set; }

    [JsonProperty("telefono")]
    public string Telefono { get; set; }

    [JsonProperty("id")]
    public string CentroSaludAraucaniaId { get; set; }
}
