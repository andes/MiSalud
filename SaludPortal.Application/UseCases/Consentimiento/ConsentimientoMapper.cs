using AndesServices.DTOs;
using SaludPortal.Application.Models.Consentimiento;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.UseCases.Consentimiento;

public static class ConsentimientoConstants
{
    public const string ProgramaCuidar65 = "Cuidar65";
    public const int EdadMinimaPrograma = 65;

    public const string AvisoNoElegibleTitulo = "IMPORTANTE";

    public const string AvisoNoElegibleParrafo1 =
        "Cuidar +65 está pensado para personas sin cobertura de obra social, además de los criterios de residencia y edad.";

    public const string AvisoNoElegibleParrafo2 =
        "Si tenés PAMI, ISSN, otras obras sociales o alguna prepaga, seguís contando con tu cobertura médica habitual.";
}

public static class ConsentimientoMapper
{
    public static ConsentimientoModel ToModel(ConsentimientoDto dto) => new()
    {
        Id = dto.Id ?? dto.IdInterno,
        Programa = dto.Programa ?? string.Empty,
        Version = dto.Version,
        PacienteId = dto.PacienteId ?? string.Empty,
        Aceptacion = dto.Aceptacion,
        FechaResp = DateTimeHelper.ToArgentinaTime(dto.FechaResp),
    };

    public static ConsentVersionModel ToModel(ConsentVersionDto dto) => new()
    {
        Programa = dto.Programa ?? string.Empty,
        Version = dto.Version,
        Titulo = dto.Titulo ?? string.Empty,
        Texto = dto.Texto ?? string.Empty,
        FormatoContenido = MapFormatoContenido(dto.Formato)
    };

    private static FormatoContenidoConsentimiento MapFormatoContenido(string? formato)
    {
        if (string.Equals(formato, "markdown", StringComparison.OrdinalIgnoreCase))
        {
            return FormatoContenidoConsentimiento.Markdown;
        }

        if (string.Equals(formato, "html", StringComparison.OrdinalIgnoreCase))
        {
            return FormatoContenidoConsentimiento.Html;
        }

        return FormatoContenidoConsentimiento.TextoPlano;
    }

    public static List<ConsentimientoModel> ToModelList(IEnumerable<ConsentimientoDto> consentimientos)
    {
        return consentimientos
            .Where(c => !string.IsNullOrWhiteSpace(c.Programa))
            .Select(ToModel)
            .OrderByDescending(c => c.FechaResp)
            .ToList();
    }
}

