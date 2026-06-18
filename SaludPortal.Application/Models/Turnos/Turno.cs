namespace SaludPortal.Application.Models.Turnos;

public class Turno
{
    public string Id { get; set; }
    public bool EsTeleconsulta { get; set; }
    public PacienteTurno? Paciente { get; set; }
    public TipoPrestacion? TipoPrestacion { get; set; }
    public Organizacion? Organizacion { get; set; }
    public DateTime FechaHora { get; set; }
    public int DuracionMinutos { get; set; }
    public string? Asistencia { get; set; }
    public DateTime? FechaHoraAsistencia { get; set; }
    public string? AgendaId { get; set; }
    public string? BloqueId { get; set; }
    public string? MotivoConsulta { get; set; }
    public List<Profesional>? Profesionales { get; set; }
    public bool VideoConferencia { get; set; }
    public WebexLinks? WebexLinks { get; set; }
    public string? Estado { get; set; }
}

public class PacienteTurno
{
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Documento { get; set; }
    public string? Alias { get; set; }

    public string? GetDisplayName()
    {
        var nombreAlias = string.IsNullOrEmpty(Nombre) ? Alias : Nombre;
        return $"{nombreAlias} {Apellido}";
    }
}

public class TipoPrestacion
{
    public string? ConceptId { get; set; }
    public string? Term { get; set; }
}

public class Organizacion
{
    public string? Id { get; set; }
    public string? Nombre { get; set; }
}

public class Profesional
{
    public string? Id { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
}

public class WebexLinks
{
    public string professionalLink { get; set; }
    public string patientLink { get; set; }
}