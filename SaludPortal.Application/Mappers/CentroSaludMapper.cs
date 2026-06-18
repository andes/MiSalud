using AndesServices.DTOs.CentrosDeSalud;
using SaludPortal.Application.Models.CentrosDeSalud;

namespace SaludPortal.Application.Mappers;

public static class CentroSaludMapper
{
    private const string OrigenProvincia = "provincia";
    private const string OrigenAraucania = "araucania";

    public static CentroSaludModel MapToCentroSaludModel(this CentroSaludProvincia centro)
    {
        var primerTelefono = centro.telecom?
            .Where(t => !string.IsNullOrWhiteSpace(t.valor))
            .OrderBy(t => t.ranking)
            .Select(t => t.valor)
            .FirstOrDefault();

        var telefonoContacto = centro.contacto?
            .Where(c => !string.IsNullOrWhiteSpace(c.valor) &&
                        string.Equals(c.tipo, "telefono", StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.ranking)
            .Select(c => c.valor)
            .FirstOrDefault();

        var complejidad = centro.nivelComplejidad.HasValue
            ? $"Nivel {centro.nivelComplejidad.Value}"
            : null;

        var geoReferencia = centro.direccion?.geoReferencia;
        var longitud = geoReferencia != null && geoReferencia.Count > 1 ? geoReferencia[1] : (double?)null;
        var latitud = geoReferencia != null && geoReferencia.Count > 0 ? geoReferencia[0] : (double?)null;

        return new CentroSaludModel
        {
            Id = !string.IsNullOrWhiteSpace(centro.id) ? centro.id : centro._id,
            Nombre = centro.nombre,
            Latitud = latitud,
            Longitud = longitud,
            Direccion = centro.direccion?.valor,
            Region = centro.direccion?.ubicacion?.provincia?.nombre,
            Comunidad = centro.direccion?.ubicacion?.localidad?.nombre,
            Complejidad = complejidad,
            Telefono = primerTelefono ?? telefonoContacto,
            Origen = OrigenProvincia
        };
    }

    public static CentroSaludModel MapToCentroSaludModel(this CentroSaludAraucania centro)
    {
        return new CentroSaludModel
        {
            Id = !string.IsNullOrWhiteSpace(centro.Id) ? centro.Id : centro.CentroSaludAraucaniaId,
            Nombre = centro.Nombre,
            Latitud = centro.Latitud,
            Longitud = centro.Longitud,
            Direccion = centro.Direccion,
            Region = centro.Region,
            Comunidad = centro.Comunidad,
            Complejidad = centro.Complejidad,
            Telefono = centro.Telefono,
            Origen = OrigenAraucania
        };
    }
}
