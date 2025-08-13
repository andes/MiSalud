namespace AndesServices.Entities.ViewModels
{
    public class VMFarmaciasTurno
    {
        public List<FarmaciasTurno>? LstFarmaciasTurno { get; set; }
        public List<Localidad>? LstLocalidad { get; set; }
        public Localidad? Localidad { get; set; }
        public string LocalidadId { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
    }
}
