namespace SaludPortal.Application.Models.Vacunaciones;

public class VacunacionModel
{
    public string Id { get; set; }
    public string Documento { get; set; }
    public string Apellido { get; set; }
    public string Nombre { get; set; }
    public string Sexo { get; set; }
    public string Vacuna { get; set; }
    public string Dosis { get; set; }
    public DateTime FechaAplicacion { get; set; }
    public string Efector { get; set; }
}
