using AndesServices.Entities;
using AndesServices.Interfaces;
using Blazored.Modal;
using BlazorSpinner;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using SaludPortal.Web;
using SaludPortal.Web.Components;
using SaludPortal.Web.Services;
using AndesServices.Services;
using AndesServices.Handlers;
using SaludPortal.Application.UseCases.Paciente;
using SaludPortal.Application.UseCases.Turnos;
using SaludPortal.Application.UseCases.Farmacias;
using SaludPortal.Web.Models;
using SaludPortal.Application.UseCases.Auth;
using SaludPortal.Application.UseCases.Recetas;
using SaludPortal.Application.UseCases.Vacunaciones;
using SaludPortal.Application.UseCases.HistoriaSalud;
using SaludPortal.Application.UseCases.CentrosDeSalud;
using SaludPortal.Application.UseCases.Laboratorios;
using SaludPortal.Application.UseCases.GrupoFamiliar;
using SaludPortal.Application.UseCases.Account;
using SaludPortal.Application.UseCases.Consentimiento;
using SaludPortal.Application;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<AppToastService>();
builder.Services.AddScoped<UserContext>();
builder.Services.AddScoped<ClientContextService>();
builder.Services.AddSingleton<BrowserContextCache>();
builder.Services.AddScoped<TelemetryService>();
builder.Services.AddScoped<VMFarmaciasTurno>();
builder.Services.AddSingleton<MessageService>();
builder.Services.AddSingleton<ConsentimientoContenidoRenderer>();
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
builder.Services.AddScoped<ICentrosSalud, CentrosSaludService>();
builder.Services.AddScoped<IConsentimientoProgramaCuidadoIntegral, ConsentimientoProgramaCuidadoIntegralService>();

// Casos de uso
builder.Services.AddScoped<ObtenerTurnosUseCase>();
builder.Services.AddScoped<ObtenerHistorialTurnosUseCase>();
builder.Services.AddScoped<RegistrarTurnoUseCase>();
builder.Services.AddScoped<SolicitarTeleconsultaUseCase>();
builder.Services.AddScoped<CancelarTurnoUseCase>();
builder.Services.AddScoped<ObtenerTurnosDisponiblesUseCase>();
builder.Services.AddScoped<ObtenerPacienteUseCase>();
builder.Services.AddScoped<ObtenerFarmaciasTurnoUseCase>();
builder.Services.AddScoped<ObtenerLocalidadesDeFarmaciasUseCase>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<ValidarConexionXroadssUseCase>();
builder.Services.AddScoped<ObtenerRecetasUseCase>();
builder.Services.AddScoped<ObtenerVacunacionesUseCase>();
builder.Services.AddScoped<ObtenerCategoriasHistoriaSaludUseCase>();
builder.Services.AddScoped<ObtenerPrestacionesHistoriaSaludUseCase>();
builder.Services.AddScoped<ObtenerDetallePrestacionUseCase>();
builder.Services.AddScoped<ObtenerUrlImagenPrestacionUseCase>();
builder.Services.AddScoped<DescargarPdfPrestacionUseCase>();
builder.Services.AddScoped<ObtenerDireccionPacienteUseCase>();
builder.Services.AddScoped<ObtenerCentrosSaludUseCase>();
builder.Services.AddScoped<ObtenerTodosLosLaboratoriosUseCase>();
builder.Services.AddScoped<DescargarInformeLaboratorioUseCase>();
builder.Services.AddScoped<ObtenerGrupoFamiliarUseCase>();
builder.Services.AddScoped<RegistrarCuentaUseCase>();
builder.Services.AddScoped<ValidarCodigoActivacionUseCase>();
builder.Services.AddScoped<SolicitarRecuperacionContraseniaUseCase>();
builder.Services.AddScoped<ReestablecerContraseniaUseCase>();
builder.Services.AddScoped<EvaluarConsentimientoProgramaUseCase>();
builder.Services.AddScoped<GuardarRespuestaConsentimientoUseCase>();
builder.Services.AddScoped<ObtenerConsentimientosUseCase>();
builder.Services.AddScoped<ObtenerVersionProgramaUseCase>();

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

builder.Services
    .AddOptions<TurnosConfiguracion>()
    .BindConfiguration(TurnosConfiguracion.SectionName);

builder.Services.AddSingleton
    (sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<TurnosConfiguracion>>().Value);

var adminLogsApiKey = builder.Configuration["AdminLogs:ApiKey"];
if (!string.IsNullOrEmpty(adminLogsApiKey))
{
    builder.Services
        .AddOptions<AdminIngestionOptions>()
        .BindConfiguration(AdminIngestionOptions.SectionName);

    var httpTimeoutSeconds = builder.Configuration.GetValue("AdminLogs:HttpTimeoutSeconds", 10);

    builder.Services.AddHttpClient<AdminApiClient>(client =>
    {
        // In Development: AdminLogs:BaseUrl (e.g. https://localhost:7180).
        // In Docker: override with AdminLogs__BaseUrl=http://admin:8080.
        var adminUrl = builder.Configuration["AdminLogs:BaseUrl"];
        if (string.IsNullOrWhiteSpace(adminUrl))
            throw new InvalidOperationException(
                "AdminLogs:BaseUrl is required when AdminLogs:ApiKey is set.");
        client.BaseAddress = new Uri(adminUrl);
        client.Timeout = TimeSpan.FromSeconds(Math.Max(1, httpTimeoutSeconds));
        client.DefaultRequestHeaders.Add("X-Api-Key", adminLogsApiKey);
    });

    builder.Services.AddSingleton<AdminIngestionQueue>();
    builder.Services.AddHostedService<AdminIngestionWorker>();
    builder.Services.AddSingleton<ILoggerProvider, ApiLoggerProvider>();
}

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
