using System.Text.Json;
using System.Text.Json.Serialization;

namespace AndesServices.Entities
{
    public class Registro
    {
        public string? _id { get; set; }
        public string? id { get; set; }
        public string? nombre { get; set; }
        public ConceptoExtendido? concepto { get; set; }
        public JsonElement? valor { get; set; }
        public bool? esSolicitud { get; set; }
        public bool? esDiagnosticoPrincipal { get; set; }
        public bool? destacado { get; set; }
        public bool? isEmpty { get; set; }
        public List<string>? relacionadoCon { get; set; }
        public List<Registro>? registros { get; set; }
        public DateTime? createdAt { get; set; }
        public UsuarioAuditoria? createdBy { get; set; }
        public DateTime? updatedAt { get; set; }
        public UsuarioAuditoria? updatedBy { get; set; }
        //public Privacy? privacy { get; set; }
        //public bool? destacado { get; set; }
        //public bool? esSolicitud { get; set; }
        //public bool? esDiagnosticoPrincipal { get; set; }
        //public List<string>? relacionadoCon { get; set; }
        //public string? _id { get; set; }
        //public string? elementoRUP { get; set; }
        //public string? nombre { get; set; }
        //public Concepto? concepto { get; set; }

        //public JsonElement? valor { get; set; }
        //public List<Registro>? registros { get; set; }
        //public bool? hasSections { get; set; }
        //public bool? isSection { get; set; }
        //public bool? noIndex { get; set; }
        //public bool? isEmpty { get; set; }
        //public DateTime? createdAt { get; set; }
        //public UsuarioAuditoria? createdBy { get; set; }
        //public DateTime? updatedAt { get; set; }
        //public UsuarioAuditoria? updatedBy { get; set; }
        //public string? id { get; set; }
    }
    public class Privacy
    {
        public string? scope { get; set; }
    }
}
