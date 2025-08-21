using System.Text.Json.Serialization;

namespace AndesServices.Entities
{
    public class EstadoPrestacion
    {
        public string? idOrigenModifica { get; set; }
        public string? motivoRechazo { get; set; }
        public string? observaciones { get; set; }
        public string? _id { get; set; }
        public string? tipo { get; set; }

        public DateTime? createdAt { get; set; }
        public UsuarioAuditoria? createdBy { get; set; }
        public string? id { get; set; }
    }
}
