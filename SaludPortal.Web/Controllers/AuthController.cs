using AndesServices.Services;
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
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginService> _logger;

        public AuthController(
            CustomAuthenticationStateProvider authStateProvider,
            IConfiguration configuration,
            ILogger<LoginService> logger)
        {
            _authStateProvider = authStateProvider;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("Datos inválidos");
                LoginService loginService = new(_configuration, _logger);
                var usuario = await loginService.Login(model.Email, model.Password);
                if (usuario == null || string.IsNullOrEmpty(usuario.token))
                    return Unauthorized("Credenciales inválidas");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim("PacienteId", usuario.pacientes?.FirstOrDefault()?.id ?? string.Empty),
                    new Claim("Documento", usuario.documento ?? string.Empty),
                    new Claim("Token", usuario.token ?? string.Empty)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    });

                return Ok(new { message = "Login exitoso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando login para usuario: {Email}", model.Email);
                return StatusCode(500, "Error interno del servidor");
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