namespace AndesServices.Entities
{
    public class UpdatedBy
    {
        public string? id { get; set; }
        public string nombre { get; set; }
        public string? apellido { get; set; }
        public Organizacion organizacion { get; set; }
    }
}
