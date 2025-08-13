using AndesServices.Entities;
using AndesServices.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaludPortal.Web.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private const string AuthKey = "authUser";
        private const string AuthToken = "authToken";
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginService> _logger;

        public CustomAuthenticationStateProvider(IConfiguration configuration, ILogger<LoginService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Devuelve el estado de autenticación actual.    
            //var username = await _localStorage.GetItemAsStringAsync(AuthKey);
            string username = "";
            if (GlobalServices.Usuario != null && GlobalServices.Usuario.email != null)
                username = GlobalServices.Usuario.email;

            ClaimsIdentity identity;
            if (!string.IsNullOrWhiteSpace(username))
            {
                identity = new ClaimsIdentity(new[]
                {
                          new Claim(ClaimTypes.Name, username)
                      }, "saludAuth");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public async Task<bool> Login(string user, string password, Ref<string> mensaje)
        {
            LoginService loginService = new LoginService(_configuration, _logger);
            User? usuario = await loginService.Login(user, password, mensaje);

            if (usuario != null && !string.IsNullOrEmpty(usuario.token))
            {
                await MarkUserAsAuthenticated(usuario);
                return true;
            }
            else
            {
                await MarkUserAsLoggedOut();
                return false;
            }
        }
        public async Task MarkUserAsAuthenticated(User usuario)
        {
            //await _localStorage.SetItemAsStringAsync(AuthKey, username);
            //await _localStorage.SetItemAsStringAsync(AuthToken, token);

            //await _localStorage.SetItemAsStringAsync(AuthToken, usuario.token);
            //await _localStorage.SetItemAsStringAsync(AuthKey, usuario.email);

            GlobalServices.Usuario = usuario;

            //var name = await _localStorage.GetItemAsync<string>(AuthKey);
            var identity = new ClaimsIdentity(new[]
            {
                      new Claim(ClaimTypes.Name, usuario.email)
                  }, "saludAuth");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            User usuarioVacio = new User();
            GlobalServices.Usuario = usuarioVacio;

            var user = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}