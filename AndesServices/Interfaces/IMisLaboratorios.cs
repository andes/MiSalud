using AndesServices.DTOs.LaboratoriosRania;
using AndesServices.Entities;
using Microsoft.JSInterop.Infrastructure;

namespace AndesServices.Interfaces
{
    public interface IMisLaboratorios
    {
        Task<bool> RegistrarLaboratorioAsync(string idProtocolo, string documento, string apellido, string nombre, string codigoHIV, string fechanacimiento, string sexobiologico, string numero, string fecha, string laboratorio, string medicoSolicitante, string efectorSolicitante, string origen, string tipoMuestra);
        Task<bool> ActualizarLaboratorioAsync(string idProtocolo, string documento, string apellido, string nombre, string codigoHIV, string fechanacimiento, string sexobiologico, string numero, string fecha, string laboratorio, string medicoSolicitante, string efectorSolicitante, string origen, string tipoMuestra);
        Task<bool> EliminarLaboratorioAsync(string idProtocolo);
        Task<List<MisLaboratorios>> ObtenerMisLaboratoriosAsync(string pacienteId, string fechaDde, string fechaHta);
        Task<List<MisLaboratorios>> ObtenerMisLaboratoriosAsync(string documento);
        Task<List<MisLaboratoriosCDA>> ObtenerMisLaboratoriosCDAAsync(string pacienteId, string fechaDde, string fechaHta);
        Task<MisLaboratorios> ObtenerLaboratorioPorIdAsync(string idProtocolo);
        Task<Byte[]> DescargarLaboratorioPorIdAsync(string idProtocolo, string documento);

        Task<Byte[]> DescargarLaboratorioCDAPorIdAsync(string documento);

        Task<List<LaboratoriosLachybs>> ObtenerMisLaboratoriosLACHYBSAsync(string usuario, string clave, string documento);

        Task<string> DescargarLaboratorioLACHyBSPorIdAsync(string usuario, string clave, string idProtocolo);

        Task<List<ProtocoloRaniaResponseDto>> ObtenerProtocolosRania(string dni);
        Task<InformeRaniaResponseDto?> ObtenerInformeRania(string protocoloId);
        Task<Byte[]> DescargarInformeRaniaPorIdAsync(string protocoloId);
    }
}
