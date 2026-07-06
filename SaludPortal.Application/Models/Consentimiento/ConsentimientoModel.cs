namespace SaludPortal.Application.Models.Consentimiento;

public class ConsentimientoModel
{
    public string? Id { get; set; }
    public string Programa { get; set; } = string.Empty;
    public int Version { get; set; }
    public string PacienteId { get; set; } = string.Empty;
    public bool Aceptacion { get; set; }
    public DateTime FechaResp { get; set; }
}
