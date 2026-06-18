namespace SaludPortal.Application.Models.HistoriaSalud;

public class CategoriaHistoriaSaludModel
{
    public string? Id { get; set; }
    public string? Titulo { get; set; }
    public string? ExpresionSnomed { get; set; }
    public string? BusquedaPor { get; set; }
    public bool DescargaAdjuntos { get; set; }
}
