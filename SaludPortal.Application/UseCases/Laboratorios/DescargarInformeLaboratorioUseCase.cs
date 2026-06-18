using AndesServices.Interfaces;

namespace SaludPortal.Application.UseCases.Laboratorios;

public class DescargarInformeLaboratorioUseCase
{
    private readonly IMisLaboratorios _laboratoriosService;

    public DescargarInformeLaboratorioUseCase(IMisLaboratorios laboratoriosService)
    {
        _laboratoriosService = laboratoriosService;
    }

    public async Task<byte[]?> EjecutarAsync(string idProtocolo, string tipo, string documento, List<string>? cdaAdjuntos = null)
    {
        switch (tipo)
        {
            case "raña":
                return await _laboratoriosService.DescargarInformeRaniaPorIdAsync(idProtocolo);

            case "cda":
                if (cdaAdjuntos == null || cdaAdjuntos.Count == 0 || string.IsNullOrEmpty(cdaAdjuntos[0]))
                    return null;

                var adjunto = cdaAdjuntos[0];
                var idDescarga = adjunto.Substring(adjunto.LastIndexOf('/') + 1);
                idDescarga = idDescarga.Substring(0, idDescarga.LastIndexOf('.'));
                return await _laboratoriosService.DescargarLaboratorioCDAPorIdAsync(idDescarga);

            default: // rup
                return await _laboratoriosService.DescargarLaboratorioPorIdAsync(idProtocolo, documento);
        }
    }
}
