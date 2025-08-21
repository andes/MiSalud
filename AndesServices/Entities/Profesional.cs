using System.Text.Json.Serialization;

namespace AndesServices.Entities
{
    public class Profesional
    {
        public string? _id { get; set; }
        public string? id { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? documento { get; set; }
        public string? profesion { get; set; }
        public string? especialidad { get; set; }
        public int? matricula { get; set; }
    }
}
