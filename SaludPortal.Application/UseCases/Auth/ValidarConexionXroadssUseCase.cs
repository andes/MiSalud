using AndesServices.Entities;
using AndesServices.Interfaces;

namespace SaludPortal.Application.UseCases.Auth;

public class ValidarConexionXroadssUseCase
{
    private readonly ILoginService<User> _loginService;

    public ValidarConexionXroadssUseCase(ILoginService<User> loginService)
    {
        _loginService = loginService;
    }

    public async Task<bool> EjecutarAsync()
    {
        return await _loginService.ValidarConexionXroadss();
    }
}
