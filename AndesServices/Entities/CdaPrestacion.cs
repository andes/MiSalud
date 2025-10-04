namespace AndesServices.Entities
{
    public class CdaPrestacion
    {
        // Reutiliza Snomed de MisTurnos.cs (conceptId, term, fsn, semanticTag)
        public Snomed? snomed { get; set; }
        public CdaLoinc? loinc { get; set; }
        public string? _id { get; set; }
    }
}
