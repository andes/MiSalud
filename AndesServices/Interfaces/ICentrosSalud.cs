using AndesServices.DTOs.CentrosDeSalud;

namespace AndesServices.Interfaces;

public interface ICentrosSalud
{
    Task<List<CentroSaludAraucania>> ObtenerCentrosDeSaludAraucania();
    Task<List<CentroSaludProvincia>> ObtenerCentrosDeSaludProvincia();
}
