using AndesServices.Entities;
using AndesServices.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace SaludPortal.Web.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILogger<LoginService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal _cachedUser = new(new ClaimsIdentity());
        private readonly IConfiguration? _configuration;

        public CustomAuthenticationStateProvider(ILogger<LoginService> logger
            , IHttpContextAccessor httpContextAccessor
            , IConfiguration? configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task Logout()
        {
            var http = _httpContextAccessor.HttpContext;
            if (http == null)
            {
                _logger.LogWarning("HttpContext null en Logout");
            }
            else
            {
                await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                http.Response.Cookies.Delete(SaludConstantes.CookieName);
            }
            _cachedUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var http = _httpContextAccessor.HttpContext;
                var user = http?.User ?? new ClaimsPrincipal(new ClaimsIdentity());

                _cachedUser = user.Identity?.IsAuthenticated == true
                    ? user
                    : new ClaimsPrincipal(new ClaimsIdentity());

                return Task.FromResult(new AuthenticationState(_cachedUser));

            }
            catch (Exception ex)
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }
        }
        public void ForceRefresh() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        private static IEnumerable<Claim> BuildClaims(string username, User usuario) =>
            new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("PacienteId", usuario.pacientes?.FirstOrDefault()?.id ?? string.Empty),
                new Claim("Documento", usuario.documento ?? string.Empty),
                new Claim("TokenBackend", usuario.token ?? string.Empty) // opcional: renombrado para no confundir con auth interno
            };
    }

}