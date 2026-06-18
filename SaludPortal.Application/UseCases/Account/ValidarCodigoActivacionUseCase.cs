using AndesServices.DTOs.Login;
using AndesServices.Entities;
using AndesServices.Interfaces;

namespace SaludPortal.Application.UseCases.Account;

public class ValidarCodigoActivacionUseCase
{
    private readonly ILoginService<User> _loginService;

    public ValidarCodigoActivacionUseCase(ILoginService<User> loginService)
    {
        _loginService = loginService;
    }

    public async Task<(bool valid, bool needsPassword, string? error)> EjecutarAsync(string email, string codigoActivacion)
    {
        var result = await _loginService.ValidarCodigoActivacion(new ValidarCodigoActivacionRequestDto
        {
            Email = email,
            CodigoActivacion = codigoActivacion
        });

        if (result == null)
            return (false, false, "Error al validar el código de activación.");

        if (result.Error != null)
            return (false, false, result.Error);

        if (result.Message != "new_password_needed")
            return (false, false, "Código de activación inválido.");

        return (true, true, null);
    }
}
