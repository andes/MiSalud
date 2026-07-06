using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SaludPortal.Web.Services;

public class UserContext
{
    private readonly AuthenticationStateProvider _auth;
    public UserContext(AuthenticationStateProvider auth) => _auth = auth;

    public async Task<ClaimsPrincipal> GetUserAsync() => (await _auth.GetAuthenticationStateAsync()).User;
    public async Task<string?> GetPacienteIdAsync() => (await GetUserAsync()).GetPacienteId();
    public async Task<string?> GetDocumentoAsync() => (await GetUserAsync()).GetDocumento();
    public async Task<string?> GetTokenAsync() => (await GetUserAsync()).GetBackendToken();
    public async Task<string?> GetSexoAsync() => (await GetUserAsync()).GetSexo();
}