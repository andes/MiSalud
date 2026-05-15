namespace AndesServices.Entities
{
    public class MisTurnos
    {
        public Diagnostico diagnostico { get; set; }
        public EstadoFacturacion estadoFacturacion { get; set; }
        public string estado { get; set; }
        public string _id { get; set; }
        public DateTime horaInicio { get; set; }
        public bool auditable { get; set; }
        public string emitidoPor { get; set; }
        public DateTime fechaHoraDacion { get; set; }
        public string link { get; set; }
        public string motivoConsulta { get; set; }
        public string nota { get; set; }
        public Paciente paciente { get; set; }
        public TipoPrestacion tipoPrestacion { get; set; }
        public string tipoTurno { get; set; }
        public UsuarioDacion usuarioDacion { get; set; }
        public string asistencia { get; set; }
        public string profesional { get; set; }
        public DateTime? horaAsistencia { get; set; }
        public List<Profesional> profesionales { get; set; }
        public Organizacion organizacion { get; set; }
        public object espacioFisico { get; set; }
        public string agenda_id { get; set; }
        public int duracionTurno { get; set; }
        public string bloque_id { get; set; }
        public string agenda_estado { get; set; }
        public string avisoSuspension { get; set; }
        public string motivoSuspension { get; set; }
        public bool? videoConferencia { get; set; }
        public WebexLinks? webexLinks { get; set; }
    }

    public class OrganizacionAgenda
    {
        public IdObject _id { get; set; }
        public string id { get; set; }
        public string? organizacion { get; set; }
        public List<Agenda>? agendas { get; set; }
        public string? circunferencia { get; set; }
        public CoordenadasDeMapa? coordenadasDeMapa { get; set; }
        public string? domicilio { get; set; }
        public string? distance { get; set; }
    }

    public class IdObject
    {
        public string id { get; set; }
    }

    public class TurnoUpdatedBy
    {
        public string nombre { get; set; }
        public string email { get; set; }
        public object organizacion { get; set; }
    }
    public class TurnoUsuarioDacion
    {
        public string nombre { get; set; }
        public string email { get; set; }
        public object organizacion { get; set; }
    }
    public class CoordenadasDeMapa
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
    public class Agenda
    {
        public string? _id { get; set; }
        public bool? intercalar { get; set; }
        public string? estado { get; set; }
        public bool? nominalizada { get; set; }
        public bool? dinamica { get; set; }
        public bool? multiprofesional { get; set; }
        public bool? enviarSms { get; set; }
        public List<Bloque>? bloques { get; set; }
        public string? horaInicio { get; set; }
        public string? horaFin { get; set; }
        public List<TipoPrestacion>? tipoPrestaciones { get; set; }
        public List<object>? profesionales { get; set; }
        public AgendaOrganizacion? organizacion { get; set; }
        public List<object>? avisos { get; set; }
        public List<object>? sobreturnos { get; set; }
        public List<Historial>? historial { get; set; }
        public string? createdAt { get; set; }
        public AgendaCreatedBy? createdBy { get; set; }
        public AgendaCreatedBy? updatedBy { get; set; }
        public string? updatedAt { get; set; }
        public bool? cumpleRegla { get; set; }
    }
    public class Historial
    {
        public string _id { get; set; }
        public string estado { get; set; }
        public string createdAt { get; set; }
        public AgendaCreatedBy createdBy { get; set; }
    }
    public class AgendaCreatedBy
    {
        public string id { get; set; }
        public string nombreCompleto { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public long username { get; set; }
        public long documento { get; set; }
        public AgendaOrganizacion organizacion { get; set; }
    }

    public class AgendaOrganizacion
    {
        public string _id { get; set; }
        public string nombre { get; set; }
    }

    public class Bloque
    {
        public int accesoDirectoDelDia { get; set; }
        public int accesoDirectoProgramado { get; set; }
        public int reservadoGestion { get; set; }
        public int reservadoProfesional { get; set; }
        public int cupoMobile { get; set; }
        public int restantesDelDia { get; set; }
        public int restantesProgramados { get; set; }
        public int restantesGestion { get; set; }
        public int restantesProfesional { get; set; }
        public int restantesMobile { get; set; }
        public bool pacienteSimultaneos { get; set; }
        public bool citarPorBloque { get; set; }
        public bool turnosMobile { get; set; }
        public string _id { get; set; }
        public int cantidadTurnos { get; set; }
        public string horaInicio { get; set; }
        public string horaFin { get; set; }
        public int duracionTurno { get; set; }
        public int? cantidadSimultaneos { get; set; }
        public int? cantidadBloque { get; set; }
        public List<TipoPrestacion> tipoPrestaciones { get; set; }
        public List<MisTurnos> turnos { get; set; }
    }
    public class Codificacion
    {
        public CodificacionProfesional codificacionProfesional { get; set; }
        public string _id { get; set; }
        public bool? primeraVez { get; set; }
    }

    public class CodificacionProfesional
    {
        public Snomed snomed { get; set; }
        public Cie10 cie10 { get; set; }
    }

    public class Snomed
    {
        public string conceptId { get; set; }
        public string term { get; set; }
        public string fsn { get; set; }
        public string semanticTag { get; set; }
    }

    public class Cie10
    {
        public string _id { get; set; }
        public string causa { get; set; }
        public string subcausa { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string sinonimo { get; set; }
        public bool? c2 { get; set; }
        public string reporteC2 { get; set; }
    }

    public class EstadoFacturacion
    {
        public string? tipo { get; set; }
        public string? estado { get; set; }
        public string? numeroComprobante { get; set; }
    }

    public class TipoPrestacion
    {
        public bool auditable { get; set; }
        public List<string> ambito { get; set; }
        public List<object> queries { get; set; }
        public string _id { get; set; }
        public string fsn { get; set; }
        public string semanticTag { get; set; }
        public string conceptId { get; set; }
        public string term { get; set; }
        public List<object> multiprestacion { get; set; }
        public string? nombre { get; set; }
        public string? id { get; set; }
    }

    public class UsuarioDacion
    {
        public string id { get; set; }
        public string nombreCompleto { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public long username { get; set; }
        public long documento { get; set; }
        public Organizacion organizacion { get; set; }
    }

    public class WebexLinks
    {
        public string professionalLink { get; set; }
        public string patientLink { get; set; }
    }
}
