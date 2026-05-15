using AndesServices.DTOs;
using AndesServices.Entities;

namespace AndesServices.Interfaces
{
    public interface IPaciente
    {
        Task<Paciente> ObtenerPacientePorIdAsync(string idPaciente, string? token = null);
        Task<userLocation> ObtenerGeoreferenciaPaciente(string direccion);
        Task<Paciente> ModificarDatos(string idPaciente, ActualizarPacienteDto paciente);
        Direccion? ObtenerDireccionPrioritaria(Paciente? paciente);
        ActualizarPacienteDireccionDto? ObtenerDireccionPrioritaria(ActualizarPacienteDto paciente);
    }
}
