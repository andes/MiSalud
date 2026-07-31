namespace SaludPortal.Application.Models.Paciente;

public class PacienteModel
{
    public string? Id { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Alias { get; set; }
    public string? NombreCompleto { get; set; }
    public string? Documento { get; set; }
    public string? NumeroIdentificacion { get; set; }
    public string? TipoIdentificacion { get; set; }
    public string? Sexo { get; set; }
    public string? Genero { get; set; }
    public string? EstadoCivil { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public DateTime? FechaFallecimiento { get; set; }
    public int? Edad { get; set; }
    public EdadRealModel? EdadReal { get; set; }
    public string? Telefono { get; set; }
    public string? Cuil { get; set; }
    public string? FotoId { get; set; }
    public string? Estado { get; set; }
    public bool? Activo { get; set; }
    public DateTime? CreadoEn { get; set; }
    public DateTime? ActualizadoEn { get; set; }
    public DateTime? AgregadoEn { get; set; }
    public string? NombreObraSocial { get; set; }
    public LugarNacimientoModel? LugarNacimiento { get; set; }
    public List<ContactoModel>? Contactos { get; set; }
    public List<DireccionModel>? Direcciones { get; set; }
    public List<IdentificadorModel>? Identificadores { get; set; }
    public List<FinanciadorModel>? Financiadores { get; set; }
    public List<DocumentoAdjuntoModel>? Documentos { get; set; }
    public List<RelacionModel>? Relaciones { get; set; }
    public List<string>? Vinculos { get; set; }

    public DireccionModel? ObtenerDireccionPrioritaria()
    {
        if (Direcciones?.Count == 1) return Direcciones[0];
        if (Direcciones?.Count > 1) return Direcciones[1];
        return null;
    }
}

public class UbicacionModel
{
    public ReferenciaModel? Pais { get; set; }
    public ReferenciaModel? Provincia { get; set; }
    public ReferenciaModel? Localidad { get; set; }
    public ReferenciaModel? Barrio { get; set; }
}

public class ReferenciaModel
{
    public string? Id { get; set; }
    public string? Nombre { get; set; }
}

public class LugarNacimientoModel
{
    public ReferenciaModel? Pais { get; set; }
}

public class IdentificadorModel
{
    public string? Entidad { get; set; }
    public string? Valor { get; set; }
}

public class FinanciadorModel
{
    public int? CodigoPuco { get; set; }
    public string? Nombre { get; set; }
    public string? Origen { get; set; }
    public DateTime FechaDeActualizacion { get; set; }
    public bool Prepaga { get; set; }
}

public class EdadRealModel
{
    public int? Valor { get; set; }
    public string? Unidad { get; set; }
}

public class TipoDocumentoModel
{
    public string? Id { get; set; }
    public string? Label { get; set; }
}

public class ArchivoAdjuntoModel
{
    public string? Ext { get; set; }
}

public class DocumentoAdjuntoModel
{
    public TipoDocumentoModel? Tipo { get; set; }
    public List<ArchivoAdjuntoModel>? Archivos { get; set; }
    public DateTime Fecha { get; set; }
}

public class DireccionModel
{
    public List<double>? GeoReferencia { get; set; }
    public bool Activo { get; set; }
    public DateTime? UltimaActualizacion { get; set; }
    public int? Ranking { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Valor { get; set; }
    public UbicacionModel? Ubicacion { get; set; }
}

public class ContactoModel
{
    public bool Activo { get; set; }
    public DateTime? UltimaActualizacion { get; set; }
    public int? Ranking { get; set; }
    public string? Valor { get; set; }
    public string? Tipo { get; set; }
}

public class RelacionTipoModel
{
    public string? Nombre { get; set; }
    public string? Opuesto { get; set; }
    public bool EsConviviente { get; set; }
}

public class RelacionModel
{
    public RelacionTipoModel? Relacion { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Documento { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public DateTime? FechaFallecimiento { get; set; }
    public bool Activo { get; set; }
}
