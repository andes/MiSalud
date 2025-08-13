using Microsoft.Extensions.Primitives;

namespace AndesServices.Entities
{
    public class Edificio
    {
        public Direccion direccion { get; set; }
        public string descripcion { get; set; }
    }
}
