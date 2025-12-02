using System.Text.Json;

namespace AndesServices.Entities
{

    public class Barrio
    {
        public string? _id { get; set; }
        public string? nombre { get; set; }
        public string? id { get; set; }
    }

    public class Ubicacion
    {
        public string? _id { get; set; }
        public Pais? pais { get; set; }
        public Provincia? provincia { get; set; }
        public Localidad? localidad { get; set; }
        public Barrio? barrio { get; set; } // Cambiar de string a Barrio
        public string? id { get; set; }
    }
}
