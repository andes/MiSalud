using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IMisTurnos
    {
        Task<bool> RegistrarTurnoAsync(string token, string documento, string motivoConsulta, string profesional, string tipoPrestacion, DateTime fechaHoraDacion, string organizacionId);
        Task<bool> ActualizarTurnoAsync(string token, string idTurno, string motivoConsulta, string profesional, DateTime fechaHoraDacion);
        Task<bool> EliminarTurnoAsync(string token, string idTurno);
        Task<List<MisTurnos>> ObtenerMisTurnosAsync(string token, string documento);
        Task<MisTurnos> ObtenerTurnoPorIdAsync(string token, string idTurno);
        Task<List<OrganizacionAgenda>> ObtenerAgendasOrganizaciones(string token, string idPaciente, userLocation userLocation);
    }
}
