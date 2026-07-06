namespace SaludPortal.Application.Models.Auth;

public class ResultadoLogin
{
    public bool Exito { get; set; }
    public string? MensajeError { get; set; }
    public string? Token { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? Documento { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Sexo { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? PrimerPacienteId { get; set; }
}
