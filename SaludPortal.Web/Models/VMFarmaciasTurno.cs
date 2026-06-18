using SaludPortal.Application.Models.Farmacias;

namespace SaludPortal.Web.Models;

public class VMFarmaciasTurno
{
    public List<LocalidadModel>? LstLocalidad { get; set; }
    public LocalidadModel? Localidad { get; set; }
    public string LocalidadId { get; set; }
    public string FechaDesde { get; set; }
    public string FechaHasta { get; set; }
}
