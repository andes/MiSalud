using AndesServices.Interfaces;
using SaludPortal.Application.Mappers;
using SaludPortal.Application.Models.HistoriaSalud;

namespace SaludPortal.Application.UseCases.HistoriaSalud;

public class DetallePrestacionResult
{
    public List<PrestacionHistoriaSaludModel> Prestaciones { get; set; } = [];
    public string? FileToken { get; set; }
    public CategoriaHistoriaSaludModel? CategoriaSeleccionada { get; set; }
}

public class ObtenerDetallePrestacionUseCase
{
    private readonly IHistoriaSalud _historiaSaludService;

    public ObtenerDetallePrestacionUseCase(IHistoriaSalud historiaSaludService)
    {
        _historiaSaludService = historiaSaludService;
    }

    public async Task<DetallePrestacionResult> EjecutarAsync(string expresionSnomed, string pacienteId)
    {
        var prestacionesTask = _historiaSaludService.ObtenerPrestacionesAsync(expresionSnomed, pacienteId);
        var fileTokenTask = _historiaSaludService.ObtenerFileTokenAsync();
        var categoriasTask = _historiaSaludService.ObtenerCategoriasHistoriaSaludAsync(expresionSnomed);

        await Task.WhenAll(prestacionesTask, fileTokenTask, categoriasTask);

        var prestaciones = await prestacionesTask;
        var fileToken = await fileTokenTask;
        var categorias = await categoriasTask;

        return new DetallePrestacionResult
        {
            Prestaciones = prestaciones?.Select(p => p.MapToPrestacionHistoriaSaludModel()).ToList() ?? [],
            FileToken = fileToken,
            CategoriaSeleccionada = categorias?.FirstOrDefault()?.MapToCategoriaHistoriaSaludModel()
        };
    }
}
