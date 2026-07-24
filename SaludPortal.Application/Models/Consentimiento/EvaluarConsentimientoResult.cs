namespace SaludPortal.Application.Models.Consentimiento;

public class EvaluarConsentimientoResult
{
    public bool MostrarPopup { get; set; }
    public bool MostrarAvisoNoElegible { get; set; }
    public ConsentVersionModel? Version { get; set; }
}
