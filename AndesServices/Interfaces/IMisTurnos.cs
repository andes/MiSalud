using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IMisTurnos
    {
        Task<bool> RegistrarTurnoAsync(string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion);
        Task<bool> RegistrarTurnoTeleConsultaAsync(string idTurno, string idBloque, string idAgenda, Paciente paciente, TipoPrestacion tipoPrestacion, string motivoConsulta = "", string estado = "");
        Task<bool> ActualizarTurnoAsync(string idTurno, string motivoConsulta, string profesional, DateTime fechaHoraDacion);
        Task<bool> CancelarTurnoAsync(string idTurno, string idBloque, string idAgenda, Paciente paciente);
        Task<List<MisTurnos>> ObtenerMisTurnosAsync(string documento);
        Task<MisTurnos> ObtenerTurnoPorIdAsync(string idTurno);
        Task<List<OrganizacionAgenda>> ObtenerAgendasOrganizaciones(string idPaciente, userLocation userLocation, bool esTeleconsulta);
        Task<List<ConceptoTurneable>> ObtenerConceptosTurneablesAsync(bool esTeleConsulta = false);
    }
}
