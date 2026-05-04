using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AndesServices.DTOs;

public class ActualizarPacienteDto
{
    [JsonProperty("alias")]
    [JsonPropertyName("alias")]
    public string? Alias { get; set; }

    [JsonProperty("genero")]
    [JsonPropertyName("genero")]
    public string? Genero { get; set; }

    [JsonProperty("contacto")]
    [JsonPropertyName("contacto")]
    public List<ActualizarPacienteContactoDto> Contacto { get; set; } = [];

    [JsonProperty("direccion")]
    [JsonPropertyName("direccion")]
    public List<ActualizarPacienteDireccionDto> Direccion { get; set; } = [];
}

public class ActualizarPacienteContactoDto
{
    [JsonProperty("activo")]
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonProperty("_id")]
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonProperty("ultimaActualizacion")]
    [JsonPropertyName("ultimaActualizacion")]
    public DateTime? UltimaActualizacion { get; set; }

    [JsonProperty("ranking")]
    [JsonPropertyName("ranking")]
    public int? Ranking { get; set; }

    [JsonProperty("valor")]
    [JsonPropertyName("valor")]
    public string? Valor { get; set; }

    [JsonProperty("tipo")]
    [JsonPropertyName("tipo")]
    public string? Tipo { get; set; }

    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class ActualizarPacienteDireccionDto
{
    [JsonProperty("geoReferencia")]
    [JsonPropertyName("geoReferencia")]
    public List<double> GeoReferencia { get; set; } = [];

    [JsonProperty("activo")]
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonProperty("_id")]
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonProperty("ultimaActualizacion")]
    [JsonPropertyName("ultimaActualizacion")]
    public DateTime? UltimaActualizacion { get; set; }

    [JsonProperty("ubicacion")]
    [JsonPropertyName("ubicacion")]
    public ActualizarPacienteUbicacionDto? Ubicacion { get; set; }

    [JsonProperty("ranking")]
    [JsonPropertyName("ranking")]
    public int? Ranking { get; set; }

    [JsonProperty("codigoPostal")]
    [JsonPropertyName("codigoPostal")]
    public string? CodigoPostal { get; set; }

    [JsonProperty("valor")]
    [JsonPropertyName("valor")]
    public string? Valor { get; set; }

    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class ActualizarPacienteUbicacionDto
{
    [JsonProperty("_id")]
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonProperty("pais")]
    [JsonPropertyName("pais")]
    public ActualizarPacienteReferenciaDto? Pais { get; set; }

    [JsonProperty("provincia")]
    [JsonPropertyName("provincia")]
    public ActualizarPacienteReferenciaDto? Provincia { get; set; }

    [JsonProperty("localidad")]
    [JsonPropertyName("localidad")]
    public ActualizarPacienteReferenciaDto? Localidad { get; set; }

    [JsonProperty("barrio")]
    [JsonPropertyName("barrio")]
    public ActualizarPacienteReferenciaDto? Barrio { get; set; }

    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class ActualizarPacienteReferenciaDto
{
    [JsonProperty("_id")]
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonProperty("nombre")]
    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}