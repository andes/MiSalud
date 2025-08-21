using AndesServices.Entities;
using AndesServices.Entities.ViewModels;
using AndesServices.Interfaces;
using Blazored.Modal;
using BlazorSpinner;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
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

builder.Services.AddCascadingAuthenticationState();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.CheckConsentNeeded = context => false;
    options.Secure = CookieSecurePolicy.Always;
});

//builder.Services.AddAuthenticationCore();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.Cookie.Name = SaludConstantes.CookieName;
        o.LoginPath = "/login";
        o.AccessDeniedPath = "/login";
        o.SlidingExpiration = true;
        o.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

//builder.Services.AddDataProtection()
//    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\DataProtection-Keys"))
//    .SetApplicationName("SaludPortal");

builder.Services.AddBlazoredModal();
builder.Services.AddScoped<SpinnerService>();
builder.Services.AddScoped<UserContext>();
builder.Services.AddScoped<VMFarmaciasTurno>();
builder.Services.AddScoped<VMMisLaboratorios>();
builder.Services.AddScoped<VMHistoriaSalud>();
builder.Services.AddSingleton<MessageService>();
builder.Services.AddTransient<IEmailService, SmtpEmailService>();

builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? builder.Configuration["BaseUrl"] ?? "https://localhost:7046/")});
builder.Services.AddTransient<IMisLaboratorios, AndesServices.Services.MisLaboratoriosService>();
builder.Services.AddTransient<SaludPortal.Web.Services.MisLaboratoriosService>();
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? builder.Configuration["BaseUrl"] ?? "https://localhost:7046/");
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient("LACHYBS_NOREDIRECT", client =>
{}).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    AllowAutoRedirect = false
});

builder.Services.AddScoped(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["ApiBaseUrl"] ?? config["BaseUrl"] ?? "https://localhost:7046/";
    if (!baseUrl.EndsWith("/")) baseUrl += "/";
    return new HttpClient { BaseAddress = new Uri(baseUrl) };
});
builder.Services.AddSession();

builder.Services.AddControllers();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles("/Files");
app.UseCookiePolicy();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapControllers();
app.UseAntiforgery();
//app.UseOutputCache();
app.MapStaticAssets();
app.MapRazorPages();

//var hubPath = builder.Configuration["Blazor:ServerHubPath"] ?? "/_blazor";
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
//app.MapRazorComponents<App>()
//   .AddInteractiveServerRenderMode(o =>
//   {
//       o.Path = hubPath; // Nueva ruta del hub SignalR
//   });

// En Program.cs (en vez de AddRazorComponents/AddInteractiveServerComponents)
//builder.Services.AddServerSideBlazor();

//app.MapBlazorHub("misaludtest.andes.gob.ar/ws");
//app.MapFallbackToPage("/_Host");


app.Run();
