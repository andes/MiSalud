using AndesServices.Entities;
using AndesServices.Entities.ViewModels;
using Blazored.Modal;
using BlazorSpinner;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.Configuration;
using SaludPortal.Web;
using SaludPortal.Web.Components;
using SaludPortal.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
//var docker = builder.AddDockerfile("SaludPortal.Web.Dockerfile", "relative/context/path");

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredModal();
builder.Services.AddScoped<SpinnerService>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddSingleton<GlobalServices>();
builder.Services.AddSingleton<VMFarmaciasTurno>();
builder.Services.AddScoped<VMMisLaboratorios>();
builder.Services.AddScoped<VMHistoriaSalud>();

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
    });

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\DataProtection-Keys"))
    .SetApplicationName("SaludPortal");

builder.Services.AddServerSideBlazor().AddCircuitOptions(o =>
{
    o.DetailedErrors = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles("/Files");

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapDefaultEndpoints();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
