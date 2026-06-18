namespace SaludPortal.Application.Models.Laboratorios;

public class LaboratorioModel
{
    public string IdProtocolo { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Fecha { get; set; } = string.Empty;
    public string? Laboratorio { get; set; }
    public string? MedicoSolicitante { get; set; }
    public string? Tipo { get; set; }
    public List<string>? CdaAdjuntos { get; set; }
}
