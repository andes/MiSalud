using System.Globalization;
using System.Security.Claims;

namespace SaludPortal.Web.Services;

public static class ClaimsPrincipalExtensions
{
    public static string? GetPacienteId(this ClaimsPrincipal user) => user.FindFirst("PacienteId")?.Value;
    public static string? GetDocumento(this ClaimsPrincipal user) => user.FindFirst("Documento")?.Value;
    public static string? GetBackendToken(this ClaimsPrincipal user) => user.FindFirst("TokenBackend")?.Value ?? user.FindFirst("Token")?.Value;
    public static string? GetNombre(this ClaimsPrincipal user) => user.FindFirst("Nombre")?.Value;
    public static string? GetApellido(this ClaimsPrincipal user) => user.FindFirst("Apellido")?.Value;
    public static string? GetSessionId(this ClaimsPrincipal user) => user.FindFirst("SessionId")?.Value;
    public static string? GetAlias(this ClaimsPrincipal user) => user.FindFirst("Alias")?.Value;
    public static string? GetSexo(this ClaimsPrincipal user) => user.FindFirst("Sexo")?.Value;

    public static DateTime? GetFechaNacimiento(this ClaimsPrincipal user)
    {
        var value = user.FindFirst("FechaNacimiento")?.Value;
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha)
            ? fecha
            : null;
    }
}