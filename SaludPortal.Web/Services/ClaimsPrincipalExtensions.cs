using System.Security.Claims;

namespace SaludPortal.Web.Services;

public static class ClaimsPrincipalExtensions
{
    public static string? GetPacienteId(this ClaimsPrincipal user) => user.FindFirst("PacienteId")?.Value;
    public static string? GetDocumento(this ClaimsPrincipal user) => user.FindFirst("Documento")?.Value;
    public static string? GetBackendToken(this ClaimsPrincipal user) => user.FindFirst("TokenBackend")?.Value ?? user.FindFirst("Token")?.Value;
    public static string? GetNombre(this ClaimsPrincipal user) => user.FindFirst("Nombre")?.Value;
    public static string? GetApellido(this ClaimsPrincipal user) => user.FindFirst("Apellido")?.Value;
    public static string? GetAlias(this ClaimsPrincipal user) => user.FindFirst("Alias")?.Value;
}