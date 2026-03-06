using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace AndesServices.Handlers
{
    public class AndesTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AndesTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Obtener el token del HttpContext si está disponible
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.User != null)
            {
                var token = httpContext.User.FindFirst("TokenBackend")?.Value 
                         ?? httpContext.User.FindFirst("Token")?.Value;

                // Añadir el header Authorization solo si el token está disponible
                // Esto permite que endpoints públicos (como login) funcionen sin token
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("JWT", token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
