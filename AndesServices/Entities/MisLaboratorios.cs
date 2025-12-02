using Microsoft.AspNetCore.Mvc;

namespace AndesServices.Entities
{

    public class MisLaboratoriosResponse
    {
        public string? SP { get; set; }
        public ParametrosEntrada? ParametrosEntrada { get; set; }
        //public ParametrosSalida? ParametrosSalida { get; set; }
        public List<MisLaboratorios>? Data { get; set; }
    }
    public class ParametrosEntrada
    {
        public string @estado { get; set; }
        public string @numeroDocumento { get; set; }
        public string @fechaNacimiento { get; set; }
        public string @apellido { get; set; }
        public string @fechaDesde { get; set; }
        public string @fechaHasta { get; set; }
    }

    //public class ParametrosSalida
    //{
    //    public string Sinparámetrosdesalida { get; set; }
    //}
    public class MisLaboratorios
    {
        public string idProtocolo { get; set; }
        public string documento { get; set; }
        public string apellido { get; set; }
        public string nombre { get; set; }
        public string codigoHIV { get; set; }
        public string fechanacimiento { get; set; }
        public string sexobiologico { get; set; }
        public string numero { get; set; }
        public string fecha { get; set; }
        public string Laboratorio { get; set; }
        public string medicoSolicitante { get; set; }
        public string efectorSolicitante { get; set; }
        public string origen { get; set; }
        public string tipoMuestra { get; set; }
        public string tipo { get; set; } // cda, raña
    }
}
