namespace AndesServices.Entities
{
    public class ConceptoTurneable
    {
        public string _id { get; set; }
        public string conceptId { get; set; }
        public string term { get; set; }
        public string fsn { get; set; }
        public string semanticTag { get; set; }
        public bool noNominalizada { get; set; }
        public bool? auditable { get; set; }
        public List<string> ambito { get; set; }
        public List<object> multiprestacion { get; set; }
    }
}