using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IFarmaciasTurno
    {
        /// <summary>
        /// Obtiene las farmacias de turno para una localidad específica.
        /// </summary>
        ///// <param name="token">Token de autenticación del usuario.</param>
        /// <param name="localidadId">ID de la localidad para la cual se desean obtener las farmacias de turno.</param>
        /// <param name="fechaDesde">Fecha desde para la cual se desean obtener las farmacias de turno.</param>
        /// <param name="fechaHasta">Fecha hasta para la cual se desean obtener las farmacias de turno.</param>
        /// <returns>Lista de farmacias de turno en la localidad especificada y según las fechas indicadas.</returns>
        Task<List<FarmaciasTurno>> ObtenerFarmaciasTurnoAsync(string localidadId, string fechaDesde, string fechaHasta);
        /// <summary>
        /// Obtiene todas las localidades disponibles.
        /// </summary>
        /// <returns>Lista de localidades.</returns>
        Task<List<Localidad>> ObtenerLocalidadesAsync();
    }
}
