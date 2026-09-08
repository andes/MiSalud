namespace SaludPortal.Application;

public sealed class TurnosConfiguracion
{
    public const string SectionName = "Turnos";

    /// <summary>
    /// Minutos desde horaInicio durante los cuales un turno de videoconferencia
    /// permanece visible en Mis Turnos. Por defecto 480 (8 horas).
    /// </summary>
    public int MinutosVisualizacionTelemedicina { get; set; } = 480;
}
