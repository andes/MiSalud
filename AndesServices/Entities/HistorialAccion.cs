using System.Text.Json.Serialization;

namespace AndesServices.Entities
{
    public class HistorialAccion
    {
        public Organizacion? Organizacion { get; set; }
        public string? _id { get; set; }
        public ConceptoHistorial? tipoPrestacion { get; set; }

        public string? accion { get; set; }
        public string? descripcion { get; set; }
        public DateTime? createdAt { get; set; }
        public UsuarioAuditoria? createdBy { get; set; }
        public DateTime updatedAt { get; set; }
        public UsuarioAuditoria? updatedBy { get; set; }
        public string? id { get; set; }
    }
}
