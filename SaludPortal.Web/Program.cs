using AndesServices.Entities;
using AndesServices.Entities.ViewModels;
using AndesServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Blazored.Modal;
using BlazorSpinner;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using SaludPortal.Web;
using SaludPortal.Web.Components;
using SaludPortal.Web.Services;
using AndesServices.Services;
using AndesServices.Handlers;

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

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.Cookie.Name = SaludConstantes.CookieName;
        o.LoginPath = "/login";
        o.AccessDeniedPath = "/login";
        o.SlidingExpiration = true;
        o.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

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

// Register AndesServices using interfaces
builder.Services.AddScoped<ILoginService<User>, LoginService>();
builder.Services.AddScoped<IMisLaboratorios, MisLaboratoriosService>();
builder.Services.AddScoped<IPaciente, PacienteService>();
builder.Services.AddScoped<IFarmaciasTurno, FarmaciasTurnoService>();
builder.Services.AddScoped<IHistoriaSalud, HistoriaSaludService>();
builder.Services.AddScoped<IVacunacion, VacunacionService>();
builder.Services.AddScoped<IMisRecetas, MisRecetasService>();
builder.Services.AddScoped<IOrganizacion, OrganizacionService>();
builder.Services.AddScoped<IMisTurnos, MisTurnosService>();
builder.Services.AddScoped<ITerritorio, TerritorioService>();

// Register AndesTokenHandler as scoped to access HttpContext
builder.Services.AddTransient<AndesTokenHandler>();
builder.Services.AddTransient<XRoadCertificateHandler>();

builder.Services.AddHttpClient("API", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? builder.Configuration["BaseUrl"] ?? "https://localhost:7046/");
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    });

builder.Services.AddHttpClient("LACHYBS_NOREDIRECT", client => {})
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        AllowAutoRedirect = false
    });

// Register named HTTP client for Andes API
builder.Services.AddHttpClient("Andes", (sp, client) =>
    {
        var configuration = sp.GetRequiredService<IConfiguration>();
        var conexionServicios = new ConexionServicios();
        configuration.GetSection("urlServicios").Bind(conexionServicios);
    
        var baseUrl = conexionServicios.usarProd 
            ? conexionServicios.UrlProyectoServiciosProd 
            : conexionServicios.UrlProyectoServiciosDemo;
    
        client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36");
    })
    .AddHttpMessageHandler<AndesTokenHandler>();
builder.Services.AddHttpClient("Andes-NoJWT", (sp, client) =>
    {
        var configuration = sp.GetRequiredService<IConfiguration>();
        var conexionServicios = new ConexionServicios();
        configuration.GetSection("urlServicios").Bind(conexionServicios);

        var baseUrl = conexionServicios.usarProd
            ? conexionServicios.UrlProyectoServiciosProd
            : conexionServicios.UrlProyectoServiciosDemo;

        client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
    });
builder.Services.AddHttpClient("ApiXroadssAndes", (sp, client) =>
    {
        var baseUrl = builder.Configuration["ApiXroadssAndes:baseUrl"] ?? "https://xroadss.andes.gob.ar";
        client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
        client.DefaultRequestHeaders.Add("X-ROAD-CLIENT", "OPTIC/GOB/GOB00008/GP-SALUD");
    })
    .ConfigurePrimaryHttpMessageHandler<XRoadCertificateHandler>();
builder.Services.AddSession(o => o.IdleTimeout = TimeSpan.FromMinutes(60));
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Configuration.AddJsonFile("saludConfig.json", optional: false, reloadOnChange: true);

builder.Services
    .AddOptions<SaludConfiguracion>()
    .BindConfiguration("SaludConfiguracion")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton
    (sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<SaludConfiguracion>>().Value);

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
app.MapStaticAssets();
app.MapRazorPages();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapPost("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});

app.Run();
