using AndesServices.Services;
using AndesServices.DTOs.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SaludPortal.Web.Models.AccountViewModels;
using SaludPortal.Web.Services;
using System.Security.Claims;

namespace SaludPortal.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CustomAuthenticationStateProvider _authStateProvider; // Cambia el tipo
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LoginService> _logger;

        public AuthController(
            CustomAuthenticationStateProvider authStateProvider,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<LoginService> logger)
        {
            _authStateProvider = authStateProvider;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        private LoginService CreateLoginService()
        {
            return new LoginService(_logger, _httpClientFactory);
        }

        [HttpPost("login")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("Datos inválidos");

                var loginService = CreateLoginService();
                var usuario = await loginService.Login(model.Email, model.Password);
                if (usuario == null || string.IsNullOrEmpty(usuario.token))
                    return Unauthorized("Credenciales inválidas");

                var claims = BuildClaims(
                    model.Email,
                    usuario.pacientes?.FirstOrDefault()?.id,
                    usuario.documento,
                    usuario.token,
                    usuario?.nombre,
                    usuario?.apellido);

                await SignInAsync(claims, model.RememberMe);

                return Ok(new { message = "Login exitoso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando login para usuario: {Email}", model.Email);
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

                var loginService = CreateLoginService();
                var result = await loginService.CrearContrasenia(model);

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
                    result.response.User?.apellido
                    );

                await SignInAsync(claims, true);

                return Ok(new { message = "Contraseña creada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando crear contraseña para usuario: {Email}", model.Email);
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

        private static List<Claim> BuildClaims(string email, string? pacienteId, string? documento, string? token, string nombre, string apellido)
        {
            return new List<Claim>
            {
                new(ClaimTypes.Name, email),
                new("PacienteId", pacienteId ?? string.Empty),
                new("Documento", documento ?? string.Empty),
                new("Token", token ?? string.Empty),
                new("Nombre", nombre ?? string.Empty),
                new("Apellido", apellido ?? string.Empty)
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