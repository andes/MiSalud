using System.Text.Json.Serialization;

namespace AndesServices.Entities
{
    public class UsuarioAuditoria
    {
        public string? id { get; set; }
        public string? nombreCompleto { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public long? username { get; set; }
        public long? documento { get; set; }
        public Organizacion? organizacion { get; set; }
    }
}
