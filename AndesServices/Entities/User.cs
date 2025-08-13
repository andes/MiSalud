namespace AndesServices.Entities
{
    public class User
    {
        public string? password { get; set; }
        public bool activacionApp { get; set; }
        public string[]? permisos { get; set; }
        public string? _id { get; set; }
        public string? documento { get; set; }
        public string? sexo { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? fechaNacimiento { get; set; }
        public string? token { get; set; }
        public List<Paciente>? pacientes { get; set; }

    }
}
