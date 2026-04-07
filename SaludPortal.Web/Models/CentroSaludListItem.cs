namespace SaludPortal.Web.Models;

public class CentroSaludListItem
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public string? Direccion { get; set; }
    public string? Region { get; set; }
    public string? Comunidad { get; set; }
    public string? Complejidad { get; set; }
    public string? Telefono { get; set; }
    public string Origen { get; set; } = string.Empty;
}