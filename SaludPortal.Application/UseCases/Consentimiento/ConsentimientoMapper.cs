using AndesServices.DTOs;
using SaludPortal.Application.Models.Consentimiento;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.UseCases.Consentimiento;

public static class ConsentimientoConstants
{
    public const string ProgramaCuidar65 = "Cuidar65";
    public const int EdadMinimaPrograma = 65;
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

    public static List<ConsentimientoModel> UltimoEstadoPorPrograma(IEnumerable<ConsentimientoDto> consentimientos)
    {
        return consentimientos
            .Where(c => !string.IsNullOrWhiteSpace(c.Programa))
            .GroupBy(c => c.Programa!, StringComparer.OrdinalIgnoreCase)
            .Select(g => ToModel(g.OrderByDescending(c => c.FechaResp).First()))
            .OrderByDescending(c => c.FechaResp)
            .ToList();
    }
}
