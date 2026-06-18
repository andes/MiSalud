using AndesServices.Entities;
using SaludPortal.Application.Models.Farmacias;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.Mappers;

public static class FarmaciaMapper
{
    public static Farmacia MapToFarmacia(this FarmaciasTurno f)
    {
        return new Farmacia
        {
            Id = f.Id,
            Nombre = f.Nombre,
            Direccion = f.Direccion,
            Telefono = f.Telefono,
            Fecha = DateTimeHelper.ToArgentinaTime(f.Fecha),
            Localidad = f.Localidad,
            Latitud = f.Latitud,
            Longitud = f.Longitud
        };
    }
}
