using SaludPortal.Application.Models.Consentimiento;

namespace SaludPortal.Web.Models;

public class ConsentimientoEvaluacionCacheDto
{
    public bool MostrarPopup { get; set; }

    public bool MostrarAvisoNoElegible { get; set; }

    public ConsentVersionModel? Version { get; set; }

    public static ConsentimientoEvaluacionCacheDto FromEvaluacion(EvaluarConsentimientoResult evaluacion) => new()
    {
        MostrarPopup = evaluacion.MostrarPopup,
        MostrarAvisoNoElegible = evaluacion.MostrarAvisoNoElegible,
        Version = evaluacion.Version
    };

    public EvaluarConsentimientoResult ToEvaluacionResult() => new()
    {
        MostrarPopup = MostrarPopup,
        MostrarAvisoNoElegible = MostrarAvisoNoElegible,
        Version = Version
    };
}
