namespace SaludPortal.Application.Models.Turnos;

public sealed class TurnosPorTipoPrestacion
{
    public string TipoPrestacionConceptId { get; set; } = string.Empty;
    public string? TipoPrestacionTerm { get; set; }
    public List<Efector> Efectores { get; set; } = [];
}

public class Efector
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Domicilio { get; set; } = string.Empty;
    public List<TurnoDisponible> TurnosDisponibles { get; set; } = [];
}

public class TurnoDisponible
{
    public string Id { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public string BloqueId { get; set; } = string.Empty;
    public string TipoPrestacion { get; set; } = string.Empty;
    public string NombreEfector { get; set; } = string.Empty;
    public string DomicilioEfector { get; set; } = string.Empty;
    public string AgendaId { get; set; } = string.Empty;
    public AndesServices.Entities.TipoPrestacion? TipoPrestacionDetalle { get; set; }
}
