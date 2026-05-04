using System.Text.Json.Serialization;

namespace AndesServices.DTOs;

public class PacrienteDto
{
    [JsonPropertyName("lugarNacimiento")]
    public PacrienteLugarNacimientoDto? LugarNacimiento { get; set; }

    [JsonPropertyName("tokens")]
    public List<string> Tokens { get; set; } = [];

    [JsonPropertyName("documento_fuzzy")]
    public List<string> DocumentoFuzzy { get; set; } = [];

    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("documento")]
    public string? Documento { get; set; }

    [JsonPropertyName("cuil")]
    public string? Cuil { get; set; }

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("estado")]
    public string? Estado { get; set; }

    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonPropertyName("apellido")]
    public string? Apellido { get; set; }

    [JsonPropertyName("alias")]
    public string? Alias { get; set; }

    [JsonPropertyName("contacto")]
    public List<PacrienteContactoDto> Contacto { get; set; } = [];

    [JsonPropertyName("sexo")]
    public string? Sexo { get; set; }

    [JsonPropertyName("genero")]
    public string? Genero { get; set; }

    [JsonPropertyName("fechaNacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [JsonPropertyName("numeroIdentificacion")]
    public string? NumeroIdentificacion { get; set; }

    [JsonPropertyName("fechaFallecimiento")]
    public DateTime? FechaFallecimiento { get; set; }

    [JsonPropertyName("direccion")]
    public List<PacrienteDireccionDto> Direccion { get; set; } = [];

    [JsonPropertyName("estadoCivil")]
    public string? EstadoCivil { get; set; }

    [JsonPropertyName("relaciones")]
    public List<PacrienteRelacionDto> Relaciones { get; set; } = [];

    [JsonPropertyName("financiador")]
    public List<PacrienteFinanciadorDto> Financiador { get; set; } = [];

    [JsonPropertyName("identificadores")]
    public List<PacrienteIdentificadorDto> Identificadores { get; set; } = [];

    [JsonPropertyName("scan")]
    public string? Scan { get; set; }

    [JsonPropertyName("reportarError")]
    public bool ReportarError { get; set; }

    [JsonPropertyName("notaError")]
    public string? NotaError { get; set; }

    [JsonPropertyName("notas")]
    public List<PacrienteNotaDto> Notas { get; set; } = [];

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("updatedBy")]
    public PacrienteUpdatedByDto? UpdatedBy { get; set; }

    [JsonPropertyName("tipoIdentificacion")]
    public string? TipoIdentificacion { get; set; }

    [JsonPropertyName("fotoId")]
    public string? FotoId { get; set; }

    [JsonPropertyName("documentos")]
    public List<PacrienteDocumentoDto> Documentos { get; set; } = [];

    [JsonPropertyName("apellidoCorrectoReportado")]
    public string? ApellidoCorrectoReportado { get; set; }

    [JsonPropertyName("nombreCorrectoReportado")]
    public string? NombreCorrectoReportado { get; set; }

    [JsonPropertyName("vinculos")]
    public List<string> Vinculos { get; set; } = [];

    [JsonPropertyName("nombreCompleto")]
    public string? NombreCompleto { get; set; }

    [JsonPropertyName("edad")]
    public int Edad { get; set; }

    [JsonPropertyName("edadReal")]
    public PacrienteEdadRealDto? EdadReal { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PacrienteLugarNacimientoDto
{
    [JsonPropertyName("localidad")]
    public PacrienteReferenciaDto? Localidad { get; set; }

    [JsonPropertyName("lugar")]
    public string? Lugar { get; set; }

    [JsonPropertyName("pais")]
    public PacrienteReferenciaDto? Pais { get; set; }

    [JsonPropertyName("provincia")]
    public PacrienteReferenciaDto? Provincia { get; set; }
}

public class PacrienteReferenciaDto
{
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PacrienteContactoDto
{
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("tipo")]
    public string? Tipo { get; set; }

    [JsonPropertyName("valor")]
    public string? Valor { get; set; }

    [JsonPropertyName("ranking")]
    public int? Ranking { get; set; }

    [JsonPropertyName("ultimaActualizacion")]
    public DateTime? UltimaActualizacion { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PacrienteDireccionDto
{
    [JsonPropertyName("geoReferencia")]
    public List<double> GeoReferencia { get; set; } = [];

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("ultimaActualizacion")]
    public DateTime? UltimaActualizacion { get; set; }

    [JsonPropertyName("ubicacion")]
    public PacrienteUbicacionDto? Ubicacion { get; set; }

    [JsonPropertyName("ranking")]
    public int? Ranking { get; set; }

    [JsonPropertyName("codigoPostal")]
    public string? CodigoPostal { get; set; }

    [JsonPropertyName("valor")]
    public string? Valor { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PacrienteUbicacionDto
{
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("pais")]
    public PacrienteReferenciaDto? Pais { get; set; }

    [JsonPropertyName("provincia")]
    public PacrienteReferenciaDto? Provincia { get; set; }

    [JsonPropertyName("localidad")]
    public PacrienteReferenciaDto? Localidad { get; set; }

    [JsonPropertyName("barrio")]
    public PacrienteReferenciaDto? Barrio { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PacrienteRelacionDto
{
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("relacion")]
    public PacrienteRelacionTipoDto? Relacion { get; set; }

    [JsonPropertyName("referencia")]
    public string? Referencia { get; set; }

    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonPropertyName("apellido")]
    public string? Apellido { get; set; }

    [JsonPropertyName("documento")]
    public string? Documento { get; set; }

    [JsonPropertyName("fechaNacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [JsonPropertyName("fechaFallecimiento")]
    public DateTime? FechaFallecimiento { get; set; }

    [JsonPropertyName("numeroIdentificacion")]
    public string? NumeroIdentificacion { get; set; }

    [JsonPropertyName("fotoId")]
    public string? FotoId { get; set; }

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PacrienteRelacionTipoDto
{
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonPropertyName("opuesto")]
    public string? Opuesto { get; set; }

    [JsonPropertyName("esConviviente")]
    public bool EsConviviente { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PacrienteFinanciadorDto
{
    [JsonPropertyName("codigoPuco")]
    public int CodigoPuco { get; set; }

    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonPropertyName("financiador")]
    public string? Financiador { get; set; }

    [JsonPropertyName("origen")]
    public string? Origen { get; set; }

    [JsonPropertyName("fechaDeActualizacion")]
    public DateTime? FechaDeActualizacion { get; set; }

    [JsonPropertyName("prepaga")]
    public bool Prepaga { get; set; }
}

public class PacrienteIdentificadorDto
{
    [JsonPropertyName("entidad")]
    public string? Entidad { get; set; }

    [JsonPropertyName("valor")]
    public string? Valor { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PacrienteNotaDto
{
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PacrienteUpdatedByDto
{
    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("organizacion")]
    public string? Organizacion { get; set; }
}

public class PacrienteDocumentoDto
{
    [JsonPropertyName("tipo")]
    public PacrienteTipoDocumentoDto? Tipo { get; set; }

    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("archivos")]
    public List<PacrienteArchivoDto> Archivos { get; set; } = [];

    [JsonPropertyName("fecha")]
    public DateTime? Fecha { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PacrienteTipoDocumentoDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

public class PacrienteArchivoDto
{
    [JsonPropertyName("_id")]
    public string? IdInterno { get; set; }

    [JsonPropertyName("ext")]
    public string? Ext { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class PacrienteEdadRealDto
{
    [JsonPropertyName("valor")]
    public int Valor { get; set; }

    [JsonPropertyName("unidad")]
    public string? Unidad { get; set; }
}