using System.Globalization;
using AndesServices.DTOs.Login;
using AndesServices.Entities;
using AndesServices.Interfaces;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.UseCases.Account;

public class RegistrarCuentaUseCase
{
    private readonly ILoginService<User> _loginService;

    public RegistrarCuentaUseCase(ILoginService<User> loginService)
    {
        _loginService = loginService;
    }

    public async Task<(bool success, string? error, bool alreadyActive)> EjecutarAsync(
        string documento, char sexo, string email, string telefono,
        string apellidos, string nombres, string ejemplar, string nroTramite, DateTime fechaNacimiento)
    {
        VerificarUsuarioXroadssResponseDto? xroadssResultDto = null;

        try
        {
            xroadssResultDto = await _loginService.VerificarUsuarioXroads(documento, sexo);
        }
        catch (Exception ex)
        {
            return (false, "Error al obtener datos del Xroadss: " + ex.Message, false);
        }

        if (xroadssResultDto != null && xroadssResultDto.Resultado == "error")
        {
            return (false, xroadssResultDto.Mensaje ?? "Error desconocido", false);
        }

        if (xroadssResultDto?.Data == null)
        {
            return (false, "No se obtuvieron datos válidos del RENAPER.", false);
        }

        var data = xroadssResultDto.Data;
        var fechaXroadss = DateTimeHelper.ParseDateTime(data.FechaNacimiento?.Trim());

        if (!TextosIguales(data.Apellido, apellidos)
            || !TextosIguales(data.Nombres, nombres)
            || !string.Equals(data.Ejemplar?.Trim(), ejemplar?.Trim(), StringComparison.OrdinalIgnoreCase)
            || !TramitesIguales(data.IdTramitePrincipal, nroTramite)
            || fechaXroadss is null
            || fechaXroadss.Value.Date != fechaNacimiento.Date)
        {
            return (false, "Datos incorrectos.", false);
        }

        var nroTramiteNormalizado = (data.IdTramitePrincipal ?? string.Empty).Trim().PadLeft(11, '0');
        var scanText = $"{nroTramiteNormalizado}@{data.Apellido?.ToUpper()}@{data.Nombres?.ToUpper()}@{sexo}@{documento}@{data.Ejemplar}@{fechaNacimiento:dd/MM/yyyy}";
        var registroResult = await _loginService.Registro(new RegistroRequestDto
        {
            ScanText = scanText,
            Email = email,
            Documento = documento,
            Sexo = sexo == 'M' ? "masculino" : "femenino",
            Telefono = telefono
        });

        RegistroResponseDto? response = registroResult.response;
        string? error = registroResult.errorMessage;

        if (response == null && error != null)
        {
            if (error == "Ya existe una cuenta activa con ese e-mail")
                return (false, error, true);

            return (false, error, false);
        }

        if (response != null && response.ActivacionApp)
        {
            return (false, "Ya existe una cuenta activa con ese e-mail", true);
        }

        if (response != null && !response.ActivacionApp)
        {
            return (true, null, false);
        }

        return (false, "Error desconocido al registrar usuario.", false);
    }

    private static bool TextosIguales(string? a, string? b) =>
        CultureInfo.InvariantCulture.CompareInfo.Compare(
            (a ?? string.Empty).Trim(),
            (b ?? string.Empty).Trim(),
            CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) == 0;

    private static bool TramitesIguales(string? a, string? b) =>
        long.TryParse((a ?? string.Empty).Trim(), out var x)
        && long.TryParse((b ?? string.Empty).Trim(), out var y)
        && x == y;
}
