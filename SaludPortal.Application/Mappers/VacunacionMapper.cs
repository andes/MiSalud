using System.Globalization;
using SaludPortal.Application.DTOs.Vacunaciones;
using SaludPortal.Application.Models.Vacunaciones;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.Mappers;

public static class VacunacionMapper
{
    public static VacunacionModel MapToVacunacionModel(this VacunacionDto v)
    {
        return new VacunacionModel
        {
            Id = v.Id,
            Documento = v.Documento,
            Apellido = v.Apellido,
            Nombre = v.Nombre,
            Sexo = v.Sexo,
            Vacuna = v.Vacuna,
            Dosis = v.Dosis,
            Efector = v.Efector,
            FechaAplicacion = DateTimeHelper.ToArgentinaTime(v.FechaAplicacion)
        };
    }
}
