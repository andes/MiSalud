namespace AndesServices.Entities
{
    public class TipoSector
    {
        public List<object> refsetIds { get; set; } = new();
        public string? fsn { get; set; }
        public string? term { get; set; }
        public string? conceptId { get; set; }
        public string? semanticTag { get; set; }
    }
}
