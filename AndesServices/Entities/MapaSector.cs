namespace AndesServices.Entities
{
    public class MapaSector
    {
        public TipoSector? tipoSector { get; set; }
        public string? _id { get; set; }
        public List<MapaSector> hijos { get; set; }
        public string? nombre { get; set; }
        public string? id { get; set; }
    }
}
