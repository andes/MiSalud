namespace SaludPortal.Application.Models.Consentimiento;

public class ConsentVersionModel
{
    public string Programa { get; set; } = string.Empty;
    public int Version { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Texto { get; set; } = string.Empty;
    public FormatoContenidoConsentimiento FormatoContenido { get; set; } = FormatoContenidoConsentimiento.Html;
}
