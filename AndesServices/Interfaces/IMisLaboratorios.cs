using AndesServices.Entities;
using Microsoft.JSInterop.Infrastructure;

namespace AndesServices.Interfaces
{
    public interface IMisLaboratorios
    {
        Task<bool> RegistrarLaboratorioAsync(string token, string idProtocolo, string documento, string apellido, string nombre, string codigoHIV, string fechanacimiento, string sexobiologico, string numero, string fecha, string laboratorio, string medicoSolicitante, string efectorSolicitante, string origen, string tipoMuestra);
        Task<bool> ActualizarLaboratorioAsync(string token, string idProtocolo, string documento, string apellido, string nombre, string codigoHIV, string fechanacimiento, string sexobiologico, string numero, string fecha, string laboratorio, string medicoSolicitante, string efectorSolicitante, string origen, string tipoMuestra);
        Task<bool> EliminarLaboratorioAsync(string token, string idProtocolo);
        Task<List<MisLaboratorios>> ObtenerMisLaboratoriosAsync(string token, string pacienteId, string fechaDde, string fechaHta);
        Task<List<MisLaboratorios>> ObtenerMisLaboratoriosAsync(string token, string documento);
        Task<List<MisLaboratoriosCDA>> ObtenerMisLaboratoriosCDAAsync(string token, string pacienteId, string fechaDde, string fechaHta);
        Task<MisLaboratorios> ObtenerLaboratorioPorIdAsync(string token, string idProtocolo);
        Task<Byte[]> DescargarLaboratorioPorIdAsync(string token, string idProtocolo, string documento);

        Task<Byte[]> DescargarLaboratorioCDAPorIdAsync(string token, string documento);

        Task<List<LaboratoriosLachybs>> ObtenerMisLaboratoriosLACHYBSAsync(string usuario, string clave, string documento);
    }
}
