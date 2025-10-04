using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IHistoriaSalud
    {
        
        /// <summary>
        /// Obtiene las categorís de la historia de salud.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario.</param>
        /// <returns>Lista de categorias de la historia de salud.</returns>
        Task<List<CategoriaHistoriaSalud>> ObtenerCategoriasHistoriaSaludAsync(string token);
        /// <summary>
        /// Obtiene las categorís de la historia de salud.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario.</param>
        /// <param name="tipoPrestaciones">Tipo de prestación a consultar.</param>
        /// <param name="idPaciente">El id del paciente.</param>
        /// <param name="estado">Estado de la prestación a consultar.</param>
        /// <returns>Lista de prestaciones de la historia de salud para una categoria.</returns>
        Task<List<PrestacionHistoriaSalud>> ObtenerPrestacionesAsync(string token, string tipoPrestaciones, string idPaciente, string estado);

        Task<Byte[]> DescargarCDAFilePorIdAsync(string token, string id);
    }
}
