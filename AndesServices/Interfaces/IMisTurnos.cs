using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IMisTurnos
    {
        Task<bool> RegistrarTurnoAsync(string token, string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion);
        Task<bool> RegistrarTurnoTeleConsultaAsync(string token, string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion, string motivoConsulta = "", string estado = "");
        Task<bool> ActualizarTurnoAsync(string token, string idTurno, string motivoConsulta, string profesional, DateTime fechaHoraDacion);
        Task<bool> CancelarTurnoAsync(string token, string idTurno, string idBloque, string idAgenda, Paciente paciente);
        Task<List<MisTurnos>> ObtenerMisTurnosAsync(string token, string documento);
        Task<MisTurnos> ObtenerTurnoPorIdAsync(string token, string idTurno);
        Task<List<OrganizacionAgenda>> ObtenerAgendasOrganizaciones(string token, string idPaciente, userLocation userLocation, bool esTeleconsulta);
        Task<List<ConceptoTurneable>> ObtenerConceptosTurneablesAsync(string token, bool esTeleConsulta = false);
    }
}
