using Microsoft.Win32;
using System.Text.Json.Serialization;

namespace AndesServices.Entities
{
    public class EjecucionPrestacion
    {
        public Organizacion? organizacion { get; set; }
        public List<Registro>? registros { get; set; }
        public DateTime? fecha { get; set; }
    }
}
