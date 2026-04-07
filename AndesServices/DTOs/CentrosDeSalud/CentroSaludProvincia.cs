using Newtonsoft.Json;

namespace AndesServices.DTOs.CentrosDeSalud;

public class CentroSaludProvincia
{
    public bool activo { get; set; }
    public bool turnosMobile { get; set; }
    public string _id { get; set; }
    public string nombre { get; set; }
    public List<Contacto> contacto { get; set; }
    public Direccion direccion { get; set; }
    public List<Telecom> telecom { get; set; }
    public Codigo codigo { get; set; }
    public List<Edificio> edificio { get; set; }
    public List<MapaSectore> mapaSectores { get; set; }
    public List<UnidadesOrganizativa> unidadesOrganizativas { get; set; }
    public List<OfertaPrestacional> ofertaPrestacional { get; set; }
    public List<TrasladosEspeciale> trasladosEspeciales { get; set; }
    public string id { get; set; }
    public TipoEstablecimiento tipoEstablecimiento { get; set; }
    public int? nivelComplejidad { get; set; }
    public DateTime? fechaAlta { get; set; }
    public DateTime? fechaBaja { get; set; }
    public bool? integracionActiva { get; set; }
    public bool? showMapa { get; set; }
    public DateTime? createdAt { get; set; }
    public CreatedBy createdBy { get; set; }
    public bool? aceptaDerivacion { get; set; }
    public bool? esCOM { get; set; }
    public Configuraciones configuraciones { get; set; }
    public ZonaSanitaria zonaSanitaria { get; set; }
    public List<object> contactos { get; set; }
    public List<Servicio> servicios { get; set; }
    public PrefijosLab prefijosLab { get; set; }
    public string prefijo { get; set; }
    public bool? usaEstadisticaV2 { get; set; }
    public bool? matriculacion { get; set; }
}

public class Codigo
{
    public string _id { get; set; }
    public string sisa { get; set; }
    public string cuie { get; set; }
    public string remediar { get; set; }
    public string id { get; set; }
    public string sips { get; set; }
}

public class Configuraciones
{
    public List<Email> emails { get; set; }
    public PlanIndicaciones planIndicaciones { get; set; }
    public bool? ips { get; set; }

    [JsonProperty("arancelamiento.firma")]
    public string arancelamientofirma { get; set; }

    [JsonProperty("arancelamiento.nombre")]
    public string arancelamientonombre { get; set; }

    [JsonProperty("arancelamiento.aclaracion1")]
    public string arancelamientoaclaracion1 { get; set; }

    [JsonProperty("arancelamiento.aclaracion2")]
    public string arancelamientoaclaracion2 { get; set; }

    [JsonProperty("arancelamiento.aclaracion3")]
    public string arancelamientoaclaracion3 { get; set; }
    public string servicioEmail { get; set; }
    public int? circunferenciaKmTurno { get; set; }
}

public class Contacto
{
    public bool activo { get; set; }
    public DateTime ultimaActualizacion { get; set; }
    public int ranking { get; set; }
    public string valor { get; set; }
    public string tipo { get; set; }
    public string _id { get; set; }
    public string id { get; set; }
}

public class CreatedBy
{
    public string id { get; set; }
    public string nombreCompleto { get; set; }
    public string nombre { get; set; }
    public string apellido { get; set; }
    public int username { get; set; }
    public int documento { get; set; }
    public Organizacion organizacion { get; set; }
}

public class Direccion
{
    public List<double> geoReferencia { get; set; }
    public bool activo { get; set; }
    public string _id { get; set; }
    public string valor { get; set; }
    public string codigoPostal { get; set; }
    public int? ranking { get; set; }
    public Ubicacion ubicacion { get; set; }
    public DateTime ultimaActualizacion { get; set; }
    public string id { get; set; }
    public bool? turnosMobile { get; set; }
}

public class Edificio
{
    public string _id { get; set; }
    public string descripcion { get; set; }
    public Contacto contacto { get; set; }
    public Direccion direccion { get; set; }
    public string id { get; set; }
}

public class Email
{
    public string nombre { get; set; }
    public string email { get; set; }
}

public class Hijo
{
    public TipoSector tipoSector { get; set; }
    public string _id { get; set; }
    public List<Hijo> hijos { get; set; }
    public string nombre { get; set; }
    public string id { get; set; }
    public UnidadConcept unidadConcept { get; set; }
}

public class Localidad
{
    public string _id { get; set; }
    public string id { get; set; }
    public string nombre { get; set; }
}

public class MapaSectore
{
    public TipoSector tipoSector { get; set; }
    public string _id { get; set; }
    public List<Hijo> hijos { get; set; }
    public string nombre { get; set; }
    public UnidadConcept unidadConcept { get; set; }
    public string id { get; set; }
}

public class OfertaPrestacional
{
    public string _id { get; set; }
    public string id { get; set; }
    public Prestacion prestacion { get; set; }
}

public class Organizacion
{
    public string _id { get; set; }
    public string id { get; set; }
    public string nombre { get; set; }
}

public class Pais
{
    public string _id { get; set; }
    public string id { get; set; }
    public string nombre { get; set; }
}

public class PlanIndicaciones
{
    public int horaInicio { get; set; }
}

public class PrefijosLab
{
    public string nivel { get; set; }
    public string prefijo { get; set; }
    public string sisaRefes { get; set; }
}

public class Prestacion
{
    public bool auditable { get; set; }
    public List<string> ambito { get; set; }
    public List<object> queries { get; set; }
    public bool teleConsulta { get; set; }
    public string _id { get; set; }
    public List<object> multiprestacion { get; set; }
    public string conceptId { get; set; }
    public string term { get; set; }
    public string fsn { get; set; }
    public string semanticTag { get; set; }
    public string nombre { get; set; }
    public string id { get; set; }
}

public class Provincia
{
    public string _id { get; set; }
    public string id { get; set; }
    public string nombre { get; set; }
}
public class Servicio
{
    public string semanticTag { get; set; }
    public string conceptId { get; set; }
    public string term { get; set; }
    public string fsn { get; set; }
    public string _id { get; set; }
    public List<object> refsetIds { get; set; }
}

public class Telecom
{
    public string tipo { get; set; }
    public string valor { get; set; }
    public int ranking { get; set; }
    public DateTime ultimaActualizacion { get; set; }
    public bool? activo { get; set; }
    public string _id { get; set; }
}

public class TipoEstablecimiento
{
    public string _id { get; set; }
    public string nombre { get; set; }
    public string id { get; set; }
    public string descripcion { get; set; }
    public string clasificacion { get; set; }
    public int? idTipoEfector { get; set; }
}

public class TipoSector
{
    public List<object> refsetIds { get; set; }
    public string conceptId { get; set; }
    public string term { get; set; }
    public string fsn { get; set; }
    public string semanticTag { get; set; }
}

public class TrasladosEspeciale
{
    public string _id { get; set; }
    public string nombre { get; set; }
    public string id { get; set; }
}

public class Ubicacion
{
    public string _id { get; set; }
    public Localidad localidad { get; set; }
    public Provincia provincia { get; set; }
    public Pais pais { get; set; }
    public string id { get; set; }
    public object barrio { get; set; }
}

public class UnidadConcept
{
    public string conceptId { get; set; }
    public string term { get; set; }
    public string fsn { get; set; }
    public string semanticTag { get; set; }
    public List<object> refsetIds { get; set; }
}

public class UnidadesOrganizativa
{
    public List<object> refsetIds { get; set; }
    public string _id { get; set; }
    public string conceptId { get; set; }
    public string term { get; set; }
    public string fsn { get; set; }
    public string semanticTag { get; set; }
    public string id { get; set; }
}

public class ZonaSanitaria
{
    public string _id { get; set; }
    public string nombre { get; set; }
    public string id { get; set; }
}