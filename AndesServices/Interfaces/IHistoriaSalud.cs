using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IHistoriaSalud
    {
        
        /// <summary>
        /// Obtiene las categorís de la historia de salud.
        /// </summary>
        /// <returns>Lista de categorias de la historia de salud.</returns>
        Task<List<CategoriaHistoriaSalud>> ObtenerCategoriasHistoriaSaludAsync();
        /// <summary>
        /// Obtiene las categorís de la historia de salud.
        /// </summary>
        /// <param name="tipoPrestaciones">Tipo de prestación a consultar.</param>
        /// <param name="idPaciente">El id del paciente.</param>
        /// <param name="estado">Estado de la prestación a consultar.</param>
        /// <returns>Lista de prestaciones de la historia de salud para una categoria.</returns>
        Task<List<PrestacionHistoriaSalud>> ObtenerPrestacionesAsync(string tipoPrestaciones, string idPaciente, string estado = "validada");

        Task<Byte[]?> DescargarCDAFilePorIdAsync(string id);

        Task<Byte[]?> DescargarPdfPrestacionAsync(string idPrestacion);

        Task<string?> ObtenerFileTokenAsync();

        Task<string?> ObtenerImagenPrestacionUrlAsync(string idPrestacion, string fileToken);
    }
}
