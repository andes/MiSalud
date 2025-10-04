using System.Text.Json;
using System.Text.Json.Serialization;

namespace AndesServices.Entities
{
    public class Paciente
    {
        public List<object>? carpetaEfectores { get; set; }
        public string id { get; set; }
        public string? nombre { get; set; }
        public string? alias { get; set; }
        public string? apellido { get; set; }
        public string? documento { get; set; }
        public string? numeroIdentificacion { get; set; }
        public string? sexo { get; set; }
        public DateTime? fechaNacimiento { get; set; }
        public ObraSocial? obraSocial { get; set; }
        public string? genero { get; set; }
        public string? nombreCompleto { get; set; }
        public int? edad { get; set; }
        public EdadReal? edadReal { get; set; }
        public string? telefono { get; set; }
        public LugarNacimiento? lugarNacimiento { get; set; }
        public List<string>? tokens { get; set; }
        public List<string>? documento_fuzzy { get; set; }
        public string? _id { get; set; }
        public List<Identificador>? identificadores { get; set; }
        public List<Contacto>? contacto { get; set; }
        public List<Direccion>? direccion { get; set; }
        public List<Relacion>? relaciones { get; set; }
        public List<Financiador>? financiador { get; set; }
        public List<object>? notas { get; set; }
        public List<DocumentoAdjunto>? documentos { get; set; }
        public string? cuil { get; set; }
        public bool? activo { get; set; }
        public string? estado { get; set; }
        public string? tipoIdentificacion { get; set; }
        public DateTime? fechaFallecimiento { get; set; }
        public string? estadoCivil { get; set; }
        public string? fotoId { get; set; }
        public string? scan { get; set; }
        public bool? reportarError { get; set; }
        public string? nombreCorrectoReportado { get; set; }
        public string? apellidoCorrectoReportado { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public DateTime? addedAt { get; set; }
        public UpdatedBy? updatedBy { get; set; }
        public List<string>? vinculos { get; set; }
        public string? relacion { get; set; }
        public string? notaError { get; set; }
        public List<object> ofertePrestacional { get; set; }
        public List<object> trasladosEspeciales { get; set; }
        public List<string>? adjuntos { get; set; }
    }

    public class LugarNacimiento
    {
        public Pais pais { get; set; }
        public object provincia { get; set; }
        public object localidad { get; set; }
        public object lugar { get; set; }
    }

    public class Contacto
    {
        public bool activo { get; set; }
        public string? _id { get; set; }
        public DateTime? ultimaActualizacion { get; set; }
        public int? ranking { get; set; }
        public string? valor { get; set; }
        public string? tipo { get; set; }
        public string? id { get; set; }

    }
    public class Relacion
    {
        public string _id { get; set; }
        public RelacionTipo relacion { get; set; }
        public string referencia { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string documento { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public DateTime? fechaFallecimiento { get; set; }
        public string numeroIdentificacion { get; set; }
        public string fotoId { get; set; }
        public bool activo { get; set; }
        public string id { get; set; }
    }

    public class RelacionTipo
    {
        public string _id { get; set; }
        public string nombre { get; set; }
        public string opuesto { get; set; }
        public bool esConviviente { get; set; }
        public string id { get; set; }
    }

    public class Financiador
    {
        public int codigoPuco { get; set; }
        public string nombre { get; set; }
        public string financiador { get; set; }
        public string origen { get; set; }
        public DateTime fechaDeActualizacion { get; set; }
        public bool prepaga { get; set; }
    }

    public class Identificador
    {
        public string entidad { get; set; }
        public string valor { get; set; }
        public string id { get; set; }
    }
    public class DocumentoAdjunto
    {
        public TipoDocumento tipo { get; set; }
        public string _id { get; set; }
        public List<ArchivoAdjunto> archivos { get; set; }
        public DateTime fecha { get; set; }
        public string id { get; set; }
    }
    public class TipoDocumento
    {
        public string id { get; set; }
        public string label { get; set; }
    }
    public class ArchivoAdjunto
    {
        public string _id { get; set; }
        public string ext { get; set; }
        public string id { get; set; }
    }

}
