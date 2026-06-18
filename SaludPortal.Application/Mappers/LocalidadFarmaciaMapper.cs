using AndesServices.Entities;
using SaludPortal.Application.Models.Farmacias;

namespace SaludPortal.Application.Mappers;

public static class LocalidadFarmaciaMapper
{
    public static LocalidadModel MapToLocalidadModel(this Localidad localidad)
    {
        return new LocalidadModel
        {
            Id = localidad._id,
            LocalidadId = localidad?.localidadId ?? "",
            Nombre = localidad?.nombre ?? ""
        };
    }
}
