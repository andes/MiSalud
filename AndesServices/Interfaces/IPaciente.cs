using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IPaciente
    {
        Task<Paciente> ObtenerPacientePorIdAsync(string idPaciente);
        Task<userLocation> ObtenerGeoreferenciaPaciente(string direccion);
        Task<Paciente> ModificarDatos(string idPaciente, Paciente paciente);
        Direccion? ObtenerDireccionPrioritaria(Paciente? paciente);
    }
}
