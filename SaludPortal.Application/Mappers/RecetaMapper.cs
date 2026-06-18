using SaludPortal.Application.Models.Recetas;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.Mappers;

public static class RecetaMapper
{
    public static Receta MapToRecetaModel(this AndesServices.Entities.MisReceta r)
    {
        return new Receta
        {
            Id = r.id ?? r._id,
            FechaRegistro = DateTimeHelper.ToArgentinaTime(r.fechaRegistro),
            FechaPrestacion = DateTimeHelper.ToArgentinaTime(r.fechaPrestacion),
            Profesional = r.profesional == null ? null : new ProfesionalReceta
            {
                Nombre = r.profesional.nombre,
                Apellido = r.profesional.apellido,
                Matricula = r.profesional.matricula,
                Especialidad = r.profesional.especialidad
            },
            Medicamento = r.medicamento == null ? null : new MedicamentoReceta
            {
                Nombre = r.medicamento.concepto?.term,
                Cantidad = r.medicamento.cantidad,
                CantEnvases = r.medicamento.cantEnvases,
                Presentacion = r.medicamento.presentacion
            },
            Diagnostico = r.diagnostico?.term,
            EstadoDispensa = r.estadoDispensaActual?.tipo
        };
    }
}
