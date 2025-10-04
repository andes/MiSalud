namespace AndesServices.Entities
{
    public class CdaDocumento
    {
        public string? paciente { get; set; }                  // "5e6fca8db50f2638064d1a2c"
        public CdaPrestacion? prestacion { get; set; }         // snomed/loinc/_id
        public Profesional? profesional { get; set; }
        public Organizacion? organizacion { get; set; }
        public List<string>? adjuntos { get; set; }            // ["<folder>/<file>.pdf"]
        public DateTimeOffset? fecha { get; set; }             // "2025-04-21T03:00:00.000Z"
        public CdaExtras? extras { get; set; }                 // id (string o número), organizacion
        public string? cda_id { get; set; }                    // "68a329f6089d140f60e91b58"
    }
}
