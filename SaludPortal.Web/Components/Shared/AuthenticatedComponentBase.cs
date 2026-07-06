using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SaludPortal.Web.Components.Shared;

public abstract class AuthenticatedComponentBase : ComponentBase
{
    [CascadingParameter] private Task<AuthenticationState>? AuthStateTask { get; set; }
    [Inject] protected NavigationManager Nav { get; set; } = default!;

    protected ClaimsPrincipal User { get; private set; } = new(new ClaimsIdentity());

    protected string? PacienteId => User.FindFirst("PacienteId")?.Value;
    protected string? Documento => User.FindFirst("Documento")?.Value;
    protected string? BackendToken => User.FindFirst("TokenBackend")?.Value ?? User.FindFirst("Token")?.Value;
    protected string? Sexo => User.FindFirst("Sexo")?.Value;

    protected override async Task OnInitializedAsync()
    {
        if (AuthStateTask == null)
            return;

        var state = await AuthStateTask;
        User = state.User;

        if (User.Identity?.IsAuthenticated != true)
        {
            var returnUrl = Nav.ToBaseRelativePath(Nav.Uri);
            Nav.NavigateTo($"/login?returnUrl={Uri.EscapeDataString(string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl)}", true);
            return;
        }

        await OnAuthenticatedInitializedAsync();
    }

    protected virtual Task OnAuthenticatedInitializedAsync() => Task.CompletedTask;
}