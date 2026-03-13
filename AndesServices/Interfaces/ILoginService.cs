using AndesServices.DTOs.Login;
using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface ILoginService<T>
    {
        Task<User> Login(string email, string password, Ref<string> mensaje);
        Task<bool> Logout(string token);
        Task<bool> UpdateUser(T user);
        Task<bool> DeleteUser(string id);
        Task<T> GetUserById(string id);
        Task<List<T>> GetAllUsers();

        Task<OlvideContraseniaResponseDto?> OlvideContrasenia(OlvideContraseniaRequestDto request);
        Task<ReestablecerPasswordResponseDto?> ReestablecerPassword(ReestablecerPasswordRequestDto request);

        Task<VerificarUsuarioRenaperResponseDto?> VerificarUsuarioRenaper(string dni, char sexo);
        Task<(RegistroResponseDto? response, string? errorMessage)> Registro(RegistroRequestDto dto);
        Task<ValidarCodigoActivacionResponseDto?> ValidarCodigoActivacion(ValidarCodigoActivacionRequestDto dto);
        Task<(CrearContraseniaResponseDto? response, string? errorMessage)> CrearContrasenia(CrearContraseniaRequestDto dto);

    }
}
