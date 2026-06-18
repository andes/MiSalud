using AndesServices.DTOs.Login;
using AndesServices.Entities;
using AndesServices.Interfaces;

namespace SaludPortal.Application.UseCases.Account;

public class ReestablecerContraseniaUseCase
{
    private readonly ILoginService<User> _loginService;

    public ReestablecerContraseniaUseCase(ILoginService<User> loginService)
    {
        _loginService = loginService;
    }

    public async Task<(bool success, string? error)> EjecutarAsync(string email, string codigo, string password, string password2)
    {
        var result = await _loginService.ReestablecerPassword(new ReestablecerPasswordRequestDto
        {
            Email = email,
            Codigo = codigo,
            Password = password,
            Password2 = password2
        });

        if (result == null)
            return (false, "No se pudo procesar su solicitud en este momento. Por favor, intente nuevamente más tarde.");

        if (result.Valid)
            return (true, null);

        return (false, result.Error);
    }
}
