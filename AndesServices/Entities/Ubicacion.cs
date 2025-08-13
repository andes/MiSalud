namespace AndesServices.Entities
{
    public class Ubicacion
    {
        public string? _id { get; set; }
        public Pais? pais { get; set; }
        public Provincia? provincia { get; set; }
        public Localidad? localidad { get; set; }
        public string? barrio { get; set; }
        public string? id { get; set; }
    }
}
