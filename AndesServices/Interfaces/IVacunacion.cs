using AndesServices.Entities;
using System.Collections.Generic;

namespace AndesServices.Interfaces
{
    public interface IVacunacion
    {
        Task<bool> RegistrarVacunacionAsync(string token, string documento, string vacuna, string fechaVacuna, string dosis);
        Task<bool> ActualizarVacunacionAsync(string token, string idVacunacion, string vacuna, string fechaVacuna, string dosis);
        Task<bool> EliminarVacunacionAsync(string token, string idVacunacion);
        Task<List<Vacunacion>> ObtenerVacunacionesPorDocumentoAsync(string token, string documento);
        Task<List<Vacunacion>> ObtenerCampañasVacunacion(string token);
    }
}
