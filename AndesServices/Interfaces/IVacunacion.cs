using AndesServices.Entities;
using System.Collections.Generic;

namespace AndesServices.Interfaces
{
    public interface IVacunacion
    {
        Task<bool> RegistrarVacunacionAsync(string documento, string vacuna, string fechaVacuna, string dosis);
        Task<bool> ActualizarVacunacionAsync(string idVacunacion, string vacuna, string fechaVacuna, string dosis);
        Task<bool> EliminarVacunacionAsync(string idVacunacion);
        Task<List<Vacunacion>> ObtenerVacunacionesPorDocumentoAsync(string documento);
        Task<List<Vacunacion>> ObtenerCampañasVacunacion();
    }
}
