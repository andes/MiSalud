using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IPaciente
    {
        Task<Paciente> ObtenerPacientePorIdAsync(string token, string idPaciente);
        Task<userLocation> ObtenerGeoreferenciaPaciente(string direccion);
        Task<Paciente> ModificarDatos(string token, string idPaciente, Paciente paciente);
    }
}
