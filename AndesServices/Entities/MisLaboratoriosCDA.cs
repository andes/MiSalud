using AndesServices.Converters;
using System.Text.Json.Serialization;

namespace AndesServices.Entities
{
    public class MisLaboratoriosCDA
    {
        public string paciente { get; set; }
        public PrestacionCDA prestacion { get; set; }
        public ProfesionalCDA profesional { get; set; }
        public OrganizacionCDA organizacion { get; set; }
        public List<string> adjuntos { get; set; }
        public DateTime fecha { get; set; }
        public ExtrasCDA extras { get; set; }
        public string cda_id { get; set; }
        public string confidentialityCode { get; set; }
        public string title { get; set; }
    }

    public class OrganizacionCDA
    {
        public string nombre { get; set; }
        public string _id { get; set; }
    }

    public class PrestacionCDA
    {
        public SnomedCDA snomed { get; set; }
        public LoincCDA loinc { get; set; }
        public string _id { get; set; }
    }

    public class SnomedCDA
    {
        public string conceptId { get; set; }
        public string term { get; set; }
        public string fsn { get; set; }
        public string semanticTag { get; set; }
    }

    public class LoincCDA
    {
        public string code { get; set; }
        public string codeSystem { get; set; }
        public string codeSystemName { get; set; }
        public string displayName { get; set; }
    }

    public class ProfesionalCDA
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
    }

    public class ExtrasCDA
    {
        [JsonConverter(typeof(JsonStringOrNumberConverter))]
        public string id { get; set; }
        public string organizacion { get; set; }
    }
}
