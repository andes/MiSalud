
namespace SaludPortal.Application.Models.Turnos;

public class TurnoFechaGrupo
{
    public DateTime Fecha { get; set; }
    public int Disponibles { get; set; }
    public List<TurnoDisponible> Turnos { get; set; } = new();
}
