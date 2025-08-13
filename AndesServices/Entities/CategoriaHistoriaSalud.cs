namespace AndesServices.Entities
{
    public class CategoriaHistoriaSalud
    {
        public string? Id { get; set; }
        public string? _id { get; set; }
        public string? Titulo { get; set; }
        public string? ExpresionSnomed { get; set; }
        public bool DescargaAdjuntos { get; set; }
        public string? BusquedaPor { get; set; }
    }
}
