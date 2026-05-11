using System.Text.Json;
using System.Text.Json.Serialization;

namespace AndesServices.Entities
{
    /// <summary>
    /// Raíz: cada item del array provisto.
    /// </summary>
    public class PrestacionHistoriaSalud
    {
        public SolicitudPrestacion? solicitud { get; set; }
        public EjecucionPrestacion? ejecucion { get; set; }
        public bool? noNominalizada { get; set; }
        public List<JsonElement>? periodosCensables { get; set; }
        public string? _id { get; set; }
        public string? id { get; set; }
        public string? inicio { get; set; }
        public Paciente? paciente { get; set; }
        public List<EstadoPrestacion>? estados { get; set; }
        public string? groupId { get; set; }
        public List<MetadataEntry>? metadata { get; set; }
        public EstadoPrestacion? estadoActual { get; set; }
        public DateTimeOffset? createdAt { get; set; }
        public UsuarioAuditoria? createdBy { get; set; }
        public DateTimeOffset? updatedAt { get; set; }
        public UsuarioAuditoria? updatedBy { get; set; }

        public static bool EsCda(string? conceptId) =>
            conceptId == "90226004" || conceptId == "86273004";

        public bool EsCda() => EsCda(solicitud?.tipoPrestacion?.conceptId);

        public bool EsEcocardiograma() => solicitud?.tipoPrestacion?.conceptId == "5001000013101";
    }
}
