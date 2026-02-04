using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
});

builder.Logging.Configure(options =>
{
    options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
                                       | ActivityTrackingOptions.TraceId
                                       | ActivityTrackingOptions.ParentId
                                       | ActivityTrackingOptions.Baggage
                                       | ActivityTrackingOptions.Tags;
});

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthorization();

builder.Services.Configure<AndesServices.Entities.ConexionServicios>(
    builder.Configuration.GetSection("urlServicios"));

// Register named HTTP clients for different APIs
builder.Services.AddHttpClient("Andes", (sp, client) =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var conexionServicios = new AndesServices.Entities.ConexionServicios();
    configuration.GetSection("urlServicios").Bind(conexionServicios);
    
    var baseUrl = conexionServicios.usarProd 
        ? conexionServicios.UrlProyectoServiciosProd 
        : conexionServicios.UrlProyectoServiciosDemo;
    
    client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36");
});

builder.Services.AddHttpClient("LACHYBS_NOREDIRECT", client =>
{}).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    AllowAutoRedirect = false
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
