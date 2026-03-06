using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IMisRecetas
    {
        /// <summary>
        /// Obtiene las recetas del paciente.
        /// </summary>
        /// <param name="pacienteId">ID del paciente.</param>
        /// <returns>Lista de recetas del paciente.</returns>
        Task<List<MisReceta>> ObtenerRecetasPacienteAsync(string pacienteId);
        /// <summary>
        /// Obtiene una receta específica por su ID.
        /// </summary>
        /// <param name="recetaId">ID de la receta.</param>
        /// <returns>Receta específica.</returns>
        Task<MisReceta> ObtenerRecetaPorIdAsync(string recetaId);

        /// <summary>
        /// Actualiza el estado de una receta.
        /// </summary>
        /// <param name="recetaId">ID de la receta a actualizar.</param>
        /// <param name="nuevoEstado">Nuevo estado de la receta.</param>
        /// <returns>Resultado de la operación.</returns>
        Task<bool> ActualizarEstadoRecetaAsync(string recetaId, Estado nuevoEstado);
    }
}
