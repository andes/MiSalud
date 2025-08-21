using Microsoft.Win32;
using System.Text.Json.Serialization;

namespace AndesServices.Entities
{
    public class SolicitudPrestacion
    {
        public ConceptoExtendido? tipoPrestacion { get; set; }
        public ConceptoExtendido? tipoPrestacionOrigen { get; set; }
        public Organizacion? organizacion { get; set; }
        public Organizacion? organizacionOrigen { get; set; }
        public Profesional? profesional { get; set; }
        public Profesional? profesionalOrigen { get; set; }
        public string? ambitoOrigen { get; set; }
        public string? prestacionOrigen { get; set; }
        public DateTime? fecha { get; set; }
        public string? turno { get; set; }
        public List<Registro>? registros { get; set; }
        public bool? turneable { get; set; }
        public bool autocitado { get; set; }
        public string? reglaId { get; set; }
        public List<HistorialSolicitud>? historial { get; set; }
    }

    public class WrapperSolicitudPrestacion
    {
        public InnerSolicitudPrestacion? solicitudPrestacion { get; set; }
    }

    public class InnerSolicitudPrestacion
    {
        public bool autocitado { get; set; }
        public ConceptoExtendido? prestacionSolicitada { get; set; }
        public Organizacion? organizacionDestino { get; set; }
        public string? reglaID { get; set; }
        public string? motivo { get; set; }
        public string? indicaciones { get; set; }
        public bool? informe { get; set; }
    }
}
