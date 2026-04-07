using System.Text.Json;
using System.Text.Json.Serialization;

namespace AndesServices.DTOs.Login;

public class VerificarUsuarioXroadssResponseDto
{
    [JsonPropertyName("resultado")]
    public string Resultado { get; set; } = string.Empty;

    [JsonPropertyName("mensaje")]
    public string? Mensaje { get; set; }

    [JsonConverter(typeof(XroadssDataDtoConverter))]
    [JsonPropertyName("data")]
    public XroadssDataDto? Data { get; set; }
}

public class XroadssDataDtoConverter : JsonConverter<XroadssDataDto?>
{
    public override XroadssDataDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            // Error response: "data": [] — skip and return null
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) { }
            return null;
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            return JsonSerializer.Deserialize<XroadssDataDto>(ref reader, options);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, XroadssDataDto? value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}

public class XroadssDataDto
{
    [JsonPropertyName("id_tramite_principal")]
    public string IdTramitePrincipal { get; set; } = string.Empty;

    [JsonPropertyName("id_tramite_tarjeta_reimpresa")]
    public int IdTramiteTarjetaReimpresa { get; set; }

    [JsonPropertyName("ejemplar")]
    public string Ejemplar { get; set; } = string.Empty;

    [JsonPropertyName("vencimiento")]
    public string Vencimiento { get; set; } = string.Empty;

    [JsonPropertyName("emision")]
    public string Emision { get; set; } = string.Empty;

    [JsonPropertyName("apellido")]
    public string Apellido { get; set; } = string.Empty;

    [JsonPropertyName("nombres")]
    public string Nombres { get; set; } = string.Empty;

    [JsonPropertyName("fecha_nacimiento")]
    public string FechaNacimiento { get; set; } = string.Empty;

    [JsonPropertyName("cuil")]
    public string Cuil { get; set; } = string.Empty;

    [JsonPropertyName("calle")]
    public string Calle { get; set; } = string.Empty;

    [JsonPropertyName("numero")]
    public string Numero { get; set; } = string.Empty;

    [JsonPropertyName("piso")]
    public string Piso { get; set; } = string.Empty;

    [JsonPropertyName("departamento")]
    public string Departamento { get; set; } = string.Empty;

    [JsonPropertyName("codigo_postal")]
    public string CodigoPostal { get; set; } = string.Empty;

    [JsonPropertyName("barrio")]
    public string Barrio { get; set; } = string.Empty;

    [JsonPropertyName("monoblock")]
    public string Monoblock { get; set; } = string.Empty;

    [JsonPropertyName("ciudad")]
    public string Ciudad { get; set; } = string.Empty;

    [JsonPropertyName("municipio")]
    public string Municipio { get; set; } = string.Empty;

    [JsonPropertyName("provincia")]
    public string Provincia { get; set; } = string.Empty;

    [JsonPropertyName("pais")]
    public string Pais { get; set; } = string.Empty;

    [JsonPropertyName("nacionalidad")]
    public string Nacionalidad { get; set; } = string.Empty;

    [JsonPropertyName("codigo_fallecido")]
    public int CodigoFallecido { get; set; }

    [JsonPropertyName("mensaje_fallecido")]
    public string MensajeFallecido { get; set; } = string.Empty;

    [JsonPropertyName("fecha_fallecimiento")]
    public string FechaFallecimiento { get; set; } = string.Empty;

    [JsonPropertyName("id_ciudadano")]
    public string IdCiudadano { get; set; } = string.Empty;

    [JsonPropertyName("codigo")]
    public int Codigo { get; set; }

    [JsonPropertyName("mensaje")]
    public string Mensaje { get; set; } = string.Empty;
}
