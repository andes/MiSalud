using AndesServices.DTOs.Login;
using AndesServices.Entities;
using AndesServices.Interfaces;

namespace SaludPortal.Application.UseCases.Account;

public class SolicitarRecuperacionContraseniaUseCase
{
    private readonly ILoginService<User> _loginService;

    public SolicitarRecuperacionContraseniaUseCase(ILoginService<User> loginService)
    {
        _loginService = loginService;
    }

    public async Task<(bool success, bool needsActivation, string? error)> EjecutarAsync(string email)
    {
        var result = await _loginService.OlvideContrasenia(new OlvideContraseniaRequestDto
        {
            Email = email,
            Origen = "Portal Mi Salud"
        });

        if (result == null)
            return (false, false, "No se pudo procesar su solicitud en este momento. Por favor, intente nuevamente más tarde.");

        if (!result.Valid)
            return (false, false, result.Error);

        return (true, false, result.Error);
    }
}
