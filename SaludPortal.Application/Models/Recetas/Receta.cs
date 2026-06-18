namespace SaludPortal.Application.Models.Recetas;

public class Receta
{
    public string? Id { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime FechaPrestacion { get; set; }
    public ProfesionalReceta? Profesional { get; set; }
    public MedicamentoReceta? Medicamento { get; set; }
    public string? Diagnostico { get; set; }
    public string? EstadoDispensa { get; set; }
}

public class ProfesionalReceta
{
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public int? Matricula { get; set; }
    public string? Especialidad { get; set; }
}

public class MedicamentoReceta
{
    public string? Nombre { get; set; }
    public int Cantidad { get; set; }
    public int CantEnvases { get; set; }
    public string? Presentacion { get; set; }
}
