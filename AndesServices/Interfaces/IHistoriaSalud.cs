using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IHistoriaSalud
    {
        /// <summary>
        /// Obtiene el historial de salud del paciente.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario.</param>
        /// <param name="pacienteId">ID del paciente para el cual se desea obtener el historial de salud.</param>
        /// <returns>Lista de registros del historial de salud del paciente.</returns>
        //Task<List<HistoriaSalud>> ObtenerHistoriaSaludAsync(string token, string pacienteId);
        /// <summary>
        /// Actualiza el historial de salud del paciente.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario.</param>
        /// <param name="historiaSalud">Objeto que contiene los datos actualizados del historial de salud.</param>
        /// <returns>Resultado de la operación de actualización.</returns>
        //Task<bool> ActualizarHistoriaSaludAsync(string token, HistoriaSalud historiaSalud);
        /// <summary>
        /// Obtiene las categorís de la historia de salud.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario.</param>
        /// <returns>Lista de categorias de la historia de salud.</returns>
        Task<List<CategoriaHistoriaSalud>> ObtenerCategoriasHistoriaSaludAsync(string token);
    }
}
