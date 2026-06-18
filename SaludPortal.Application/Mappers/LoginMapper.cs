using System.Text.Json;
using AndesServices.Entities;
using SaludPortal.Application.Models.Auth;

namespace SaludPortal.Application.Mappers;

public static class LoginMapper
{
    public static ResultadoLogin MapToResultadoLogin(this User user)
    {
        return new ResultadoLogin
        {
            Exito = !string.IsNullOrEmpty(user.token),
            Token = user.token,
            Documento = user.documento,
            Nombre = user.nombre,
            Apellido = user.apellido,
            PrimerPacienteId = user.pacientes?.FirstOrDefault()?.id
        };
    }

    public static string? ExtractMensajeError(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        if (raw.TrimStart().StartsWith('{'))
        {
            try
            {
                using var doc = JsonDocument.Parse(raw);
                var root = doc.RootElement;
                if (root.TryGetProperty("error", out var errorProp))
                    return errorProp.GetString();
                if (root.TryGetProperty("message", out var msgProp))
                    return msgProp.GetString();
            }
            catch { }
        }

        return raw;
    }
}
