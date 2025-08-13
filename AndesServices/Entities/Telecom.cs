namespace AndesServices.Entities
{
    public class Telecom
    {
        public string? _id { get; set; }
        public bool activo { get; set; }
        public DateTime? ultimaActualizacion { get; set; }
        public int ranking { get; set; }
        public string? valor { get; set; }
        public string? tipo { get; set; }
    }
}
