namespace AndesServices.Entities
{
    public class FarmaciasTurno
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public DateTime Fecha { get; set; }
        public string Localidad { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
    }
}
