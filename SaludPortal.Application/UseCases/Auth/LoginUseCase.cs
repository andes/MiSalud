using AndesServices.Entities;
using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.Auth;

namespace SaludPortal.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly ILoginService<User> _loginService;

    public LoginUseCase(ILoginService<User> loginService)
    {
        _loginService = loginService;
    }

    public async Task<ResultadoLogin> EjecutarAsync(string email, string password)
    {
        var mensaje = new Ref<string>();
        var usuario = await _loginService.Login(email, password, mensaje);

        if (usuario == null || string.IsNullOrEmpty(usuario.token))
            return new ResultadoLogin { Exito = false, MensajeError = LoginMapper.ExtractMensajeError(mensaje.Value) };

        return usuario.MapToResultadoLogin();
    }
}

