namespace AndesServices.Entities
{
    public class MisReceta
    {
        public Organizacion organizacion { get; set; }
        public string _id { get; set; }
        public Profesional profesional { get; set; }
        public Diagnostico diagnostico { get; set; }
        public Medicamento medicamento { get; set; }
        public List<object>? dispensa { get; set; }
        public List<Estado>? estados { get; set; }
        public List<EstadoDispensa>? estadosDispensa { get; set; }
        public List<AppNotificada>? appNotificada { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaPrestacion { get; set; }
        public string idPrestacion { get; set; }
        public string idRegistro { get; set; }
        public Estado estadoActual { get; set; }
        public EstadoDispensa? estadoDispensaActual { get; set; }
        public Paciente? paciente { get; set; }
        public DateTime createdAt { get; set; }
        public CreatedBy? createdBy { get; set; }
        public DateTime updatedAt { get; set; }
        public UpdatedBy? updatedBy { get; set; }
        public string id { get; set; }
    }

    public class Diagnostico
    {
        public string? term { get; set; }
        public string? fsn { get; set; }
        public string? conceptId { get; set; }
        public string? semanticTag { get; set; }
        public List<object>? codificaciones { get; set; }
    }

    public class Medicamento
    {
        public DosisDiaria dosisDiaria { get; set; }
        public string _id { get; set; }
        public Concepto concepto { get; set; }
        public string presentacion { get; set; }
        public int cantidad { get; set; }
        public int cantEnvases { get; set; }
        public bool tratamientoProlongado { get; set; }
        public object tiempoTratamiento { get; set; }
        public string tipoReceta { get; set; }
        public string id { get; set; }
        public string serie { get; set; }
        public string numero { get; set; }
    }

    public class DosisDiaria
    {
        public string? dosis { get; set; }
        public Intervalo intervalo { get; set; }
        public string? dias { get; set; }
        public string? notaMedica { get; set; }
    }

    public class Intervalo
    {
        public string _id { get; set; }
        public string key { get; set; }
        public string nombre { get; set; }
        public string source { get; set; }
        public string type { get; set; }
        public string id { get; set; }
    }

    public class Concepto
    {
        public string conceptId { get; set; }
        public string term { get; set; }
        public string fsn { get; set; }
        public string semanticTag { get; set; }
    }

    public class Estado
    {
        public string tipo { get; set; }
        public string _id { get; set; }
        public DateTime createdAt { get; set; }
        public CreatedBy createdBy { get; set; }
        public string id { get; set; }
    }

    public class CreatedBy
    {
        public string id { get; set; }
        public string nombreCompleto { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public long? username { get; set; }
        public long? documento { get; set; }
        public Organizacion organizacion { get; set; }
    }

    public class EstadoDispensa
    {
        public string tipo { get; set; }
        public string _id { get; set; }
        public DateTime fecha { get; set; }
        public string id { get; set; }
    }

    public class AppNotificada
    {
        public string _id { get; set; }
        public string app { get; set; }
        public DateTime fecha { get; set; }
        public string id { get; set; }
    }
}
