namespace AndesServices.Entities
{
    public class Direccion
    {
        public List<double> geoReferencia { get; set; } = new();
        public bool activo { get; set; }
        public string? _id { get; set; }
        public DateTime? ultimaActualizacion { get; set; }
        public Ubicacion? ubicacion { get; set; }
        public int? ranking { get; set; }
        public string? codigoPostal { get; set; }
        public string? valor { get; set; }
        public string? id { get; set; }
    }
}
