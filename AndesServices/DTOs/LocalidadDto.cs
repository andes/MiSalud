using AndesServices.Entities;

namespace AndesServices.DTOs
{
    public class LocalidadDto
    {
        public bool activo { get; set; }
        public string _id { get; set; }
        public string codLocalidad { get; set; }
        public object codBahra { get; set; }
        public Provincia provincia { get; set; }
        public string departamento { get; set; }
        public string nombre { get; set; }
        public object codigoPostal { get; set; }
        public string id { get; set; }
    }
}
