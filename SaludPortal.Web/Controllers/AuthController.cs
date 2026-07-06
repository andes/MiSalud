using AndesServices.DTOs.Login;
using AndesServices.Entities;
using AndesServices.Interfaces;
using SaludPortal.Application.UseCases.Paciente;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.UseCases.Auth;
using SaludPortal.Web.Models.AccountViewModels;
using SaludPortal.Web.Services;
using System.Security.Claims;

namespace SaludPortal.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CustomAuthenticationStateProvider _authStateProvider;
        private readonly ILogger<AuthController> _logger;
        private readonly LoginUseCase _loginUseCase;
        private readonly ILoginService<User> _loginService;
        private readonly ObtenerPacienteUseCase _obtenerPacienteUseCase;

        public AuthController(
            CustomAuthenticationStateProvider authStateProvider,
            ILogger<AuthController> logger,
            LoginUseCase loginUseCase,
            ILoginService<User> loginService,
            ObtenerPacienteUseCase obtenerPacienteUseCase)
        {
            _authStateProvider = authStateProvider;
            _logger = logger;
            _loginUseCase = loginUseCase;
            _loginService = loginService;
            _obtenerPacienteUseCase = obtenerPacienteUseCase;
        }

        [HttpPost("login")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("Datos inválidos");

                var resultado = await _loginUseCase.EjecutarAsync(model.Email, model.Password);
                if (!resultado.Exito || string.IsNullOrEmpty(resultado.Token))
                    return Unauthorized(new { message = resultado.MensajeError ?? "Credenciales inválidas" });

                var paciente = !string.IsNullOrEmpty(resultado.PrimerPacienteId)
                    ? await _obtenerPacienteUseCase.EjecutarAsync(resultado.PrimerPacienteId, resultado.Token)
                    : null;

                var claims = BuildClaims(
                    resultado.Email,
                    paciente?.Id,
                    paciente?.Documento,
                    resultado.Token,
                    paciente?.Nombre,
                    paciente?.Apellido,
                    paciente?.Alias,
                    paciente?.Sexo,
                    paciente?.FechaNacimiento?.ToString("yyyy-MM-dd"));

                await SignInAsync(claims, model.RememberMe);

                return Ok(new { message = "Login exitoso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("crear-contrasenia")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CrearContrasenia([FromBody] CrearContraseniaRequestDto model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("Datos inválidos");

                var result = await _loginService.CrearContrasenia(model);

                if (result.response == null || string.IsNullOrEmpty(result.response.Token))
                    return BadRequest(new { message = result.errorMessage ?? "Error al crear contraseña" });

                if (result.response.User?.activacionApp == false)
                    return BadRequest(new { message = "Código inválido" });

                var claims = BuildClaims(
                    model.Email,
                    result.response.User?.pacientes?.FirstOrDefault()?.id,
                    result.response.User?.documento,
                    result.response.Token,
                    result.response.User?.nombre,
                    result.response.User?.apellido,
                    sexo: result.response.User?.sexo,
                    fechaNacimiento: result.response.User?.MapToResultadoLogin().FechaNacimiento?.ToString("yyyy-MM-dd"));

                await SignInAsync(claims, true);

                return Ok(new { message = "Contraseña creada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost("logout")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete(SaludConstantes.CookieName, new CookieOptions { Path = "/" });

            _authStateProvider.ForceRefresh();

            return Ok(new { message = "Logout ok" });
        }

        [HttpGet("status")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Status()
        {
            var user = HttpContext.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                return Ok(new AuthStatusDto
                {
                    Authenticated = true,
                    Name = user.Identity?.Name,
                    Claims = user.Claims.ToDictionary(c => c.Type, c => c.Value)
                });
            }
            return Ok(new AuthStatusDto { Authenticated = false });
        }

        private static List<Claim> BuildClaims(string email, string? pacienteId, string? documento, string? token, string? nombre, string? apellido, string? alias = null, string? sexo = null, string? fechaNacimiento = null)
        {
            return new List<Claim>
            {
                new(ClaimTypes.Name, email),
                new("PacienteId", pacienteId ?? string.Empty),
                new("Documento", documento ?? string.Empty),
                new("Token", token ?? string.Empty),
                new("Nombre", nombre ?? string.Empty),
                new("Apellido", apellido ?? string.Empty),
                new("SessionId", Guid.NewGuid().ToString("N")),
                new("Alias", alias ?? string.Empty),
                new("Sexo", sexo ?? string.Empty),
                new("FechaNacimiento", fechaNacimiento ?? string.Empty)
            };
        }

        private async Task SignInAsync(List<Claim> claims, bool isPersistent)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = isPersistent
                });
        }

        public class AuthStatusDto
        {
            public bool Authenticated { get; set; }
            public string? Name { get; set; }
            public Dictionary<string, string>? Claims { get; set; }
        }

        public class UserLoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Mensaje { get; set; }
        }
    }
}