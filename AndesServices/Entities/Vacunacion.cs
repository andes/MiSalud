namespace AndesServices.Entities
{
    public class Vacunacion
    {
        public string  _id { get; set; }
        public long idvacuna { get; set; }
        public string codigo { get; set; }
        public string vacuna { get; set; }
        public string dosis { get; set; }
        public long ordenDosis { get; set; }
        public string fechaAplicacion { get; set; }
        public DateTime? fechaAplicacionDate { get; set; }
        public string efector { get; set; }
        public string? esquema { get; set; }
        public string? condicion { get; set; }
        public long? codigoEsquema { get; set; }
        public long? codigoCondicion { get; set; }
        public string id { get; set; }
    }
}
