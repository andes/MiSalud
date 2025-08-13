namespace AndesServices.Entities
{
    public class Organizacion
    {
        public bool activo { get; set; }
        public bool turnosMobile { get; set; }
        public string? _id { get; set; }
        public string nombre { get; set; }
        public List<Contacto> contactos { get; set; }
        public Direccion? direccion { get; set; }
        public List<Telecom> telecom { get; set; } = new();
        public Codigo? codigo { get; set; }
        public TipoEstablecimiento? tipoEstablecimiento { get; set; }
        public int nivelComplejidad { get; set; }
    
        public bool integracionActiva { get; set; }
        public DateTime? fechaAlta { get; set; }
        public DateTime? fechaBaja { get; set; }
        public List<MapaSector> mapaSectores { get; set; } = new();
        public List<UnidadOrganizativa> unidadesOrganizativas { get; set; } = new();
        public List<object> ofertaPrestacional { get; set; } = new();
        public List<object> trasladosEspeciales { get; set; } = new();
        public string? id { get; set; }
        public PrefijosLab? prefijosLab { get; set; }
        public string? prefijo { get; set; }
        public bool showMapa { get; set; }
        public bool aceptaDerivacion { get; set; }
        public Configuraciones? configuraciones { get; set; }
        public string? servicioEmail { get; set; }
        public List<Email> emails { get; set; }
        public List<Servicio> servicios { get; set; }
        public List<Edificio> edificio { get; set; }
    }

    public class Codigo
    {
        public string? _id { get; set; }
        public string? remediar { get; set; }
        public string? cuie { get; set; }
        public string? sisa { get; set; }
        public string? id { get; set; }
        public string? sips { get; set; }
    }
}
