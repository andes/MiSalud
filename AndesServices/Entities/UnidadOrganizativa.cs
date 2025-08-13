namespace AndesServices.Entities
{
    public class UnidadOrganizativa
    {
        public List<object> refsetIds { get; set; } = new();
        public string? _id { get; set; }
        public string? fsn { get; set; }
        public string? term { get; set; }
        public string? conceptId { get; set; }
        public string? semanticTag { get; set; }
        public string? id { get; set; }
    }
}
