using System.Text.Json;
using System.Text.Json.Serialization;

namespace AndesServices.Entities
{
    public class Concepto
    {
        public string? conceptId { get; set; }
        public string? term { get; set; }
        public string? fsn { get; set; }
        public string? semanticTag { get; set; }
    }

    public class ConceptoExtendido : Concepto
    {
        public string? id { get; set; }
    }

    public class ConceptoHistorial : Concepto
    {
        public string? _id { get; set; }
        public string? id { get; set; }
        public bool? auditable { get; set; }
        public List<object>? ambito { get; set; }
        public List<object>? queries { get; set; }
        public List<object>? multiprestacion { get; set; }
        public string? nombre { get; set; }
    }
}
