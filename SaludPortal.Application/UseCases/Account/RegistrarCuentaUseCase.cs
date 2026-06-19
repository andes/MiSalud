using AndesServices.DTOs.Login;
using AndesServices.Entities;
using AndesServices.Interfaces;

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
        var idTramitePrincipal = (data.IdTramitePrincipal ?? string.Empty).Trim().PadLeft(11, '0');

        var errores = new List<string>();

        if (!string.Equals(data.Apellido?.Trim(), apellidos?.Trim(), StringComparison.OrdinalIgnoreCase))
            errores.Add("El apellido no coincide con el registrado en el RENAPER.");

        if (!string.Equals(data.Nombres?.Trim(), nombres?.Trim(), StringComparison.OrdinalIgnoreCase))
            errores.Add("El nombre no coincide con el registrado en el RENAPER.");

        if (!string.Equals(data.Ejemplar?.Trim(), ejemplar?.Trim(), StringComparison.OrdinalIgnoreCase))
            errores.Add("El ejemplar no coincide con el registrado en el RENAPER.");

        if (!string.Equals(idTramitePrincipal, nroTramite))
            errores.Add("El número de trámite no coincide con el registrado en el RENAPER.");

        if (!DateTime.TryParseExact(data.FechaNacimiento, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var fechaXroadss)
            || fechaXroadss.Date != fechaNacimiento.Date)
            errores.Add("La fecha de nacimiento no coincide con la registrada en el RENAPER.");

        if (errores.Count > 0)
        {
            return (false, string.Join(" ", errores), false);
        }

        var scanText = $"{nroTramite}@{apellidos.ToUpper()}@{nombres.ToUpper()}@{sexo}@{documento}@{ejemplar}@{fechaNacimiento:dd/MM/yyyy}";
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
}
