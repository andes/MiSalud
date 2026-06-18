using AndesServices.Interfaces;

namespace SaludPortal.Application.UseCases.HistoriaSalud;

public class DescargarPdfPrestacionUseCase
{
    private readonly IHistoriaSalud _historiaSaludService;

    public DescargarPdfPrestacionUseCase(IHistoriaSalud historiaSaludService)
    {
        _historiaSaludService = historiaSaludService;
    }

    public async Task<byte[]?> EjecutarAsync(string prestacionId, bool esCda, List<string>? adjuntos = null)
    {
        if (esCda)
        {
            var idDescarga = prestacionId;

            if (adjuntos != null && adjuntos.Count > 0 && !string.IsNullOrEmpty(adjuntos[0]))
            {
                var adjunto = adjuntos[0];
                idDescarga = adjunto.Substring(adjunto.LastIndexOf('/') + 1, adjunto.LastIndexOf('.') - adjunto.LastIndexOf('/') - 1);
            }

            return await _historiaSaludService.DescargarCDAFilePorIdAsync(idDescarga);
        }

        return await _historiaSaludService.DescargarPdfPrestacionAsync(prestacionId);
    }
}
