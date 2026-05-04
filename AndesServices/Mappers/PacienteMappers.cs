using AndesServices.DTOs;
using AndesServices.Entities;

namespace AndesServices
{
    public static class PacienteMappers
    {
        public static ActualizarPacienteDto MapToActualizarPacienteDto(this Paciente pacienteBase)
        {
            return new ActualizarPacienteDto
            {
                Alias = pacienteBase.alias,
                Genero = pacienteBase.genero,
                Contacto = pacienteBase.contacto?.Select(MapearContacto).ToList() ?? new List<ActualizarPacienteContactoDto>(),
                Direccion = pacienteBase.direccion?.Select(MapearDireccion).ToList() ?? new List<ActualizarPacienteDireccionDto>()
            };
        }
        
        private static ActualizarPacienteContactoDto MapearContacto(Contacto contacto)
        {
            return new ActualizarPacienteContactoDto
            {
                Activo = contacto.activo,
                IdInterno = contacto._id,
                UltimaActualizacion = contacto.ultimaActualizacion,
                Ranking = contacto.ranking,
                Valor = contacto.valor,
                Tipo = contacto.tipo,
                Id = contacto.id
            };
        }

        private static ActualizarPacienteDireccionDto MapearDireccion(Direccion direccion)
        {
            return new ActualizarPacienteDireccionDto
            {
                GeoReferencia = direccion.geoReferencia?.ToList() ?? new List<double>(),
                Activo = direccion.activo,
                IdInterno = direccion._id,
                UltimaActualizacion = direccion.ultimaActualizacion,
                Ubicacion = MapearUbicacion(direccion.ubicacion),
                Ranking = direccion.ranking,
                CodigoPostal = direccion.codigoPostal,
                Valor = direccion.valor,
                Id = direccion.id
            };
        }

        private static ActualizarPacienteUbicacionDto? MapearUbicacion(Ubicacion? ubicacion)
        {
            if (ubicacion == null)
            {
                return null;
            }

            return new ActualizarPacienteUbicacionDto
            {
                IdInterno = ubicacion._id,
                Pais = MapearReferencia(ubicacion.pais?._id, ubicacion.pais?.id, ubicacion.pais?.nombre),
                Provincia = MapearReferencia(ubicacion.provincia?._id, ubicacion.provincia?.id, ubicacion.provincia?.nombre),
                Localidad = MapearReferencia(ubicacion.localidad?._id, ubicacion.localidad?.id, ubicacion.localidad?.nombre),
                Barrio = MapearReferencia(ubicacion.barrio?._id, ubicacion.barrio?.id, ubicacion.barrio?.nombre),
                Id = ubicacion.id
            };
        }

        private static ActualizarPacienteReferenciaDto? MapearReferencia(string? idInterno, string? id, string? nombre)
        {
            if (string.IsNullOrEmpty(idInterno) && string.IsNullOrEmpty(id) && string.IsNullOrEmpty(nombre))
            {
                return null;
            }

            return new ActualizarPacienteReferenciaDto
            {
                IdInterno = idInterno,
                Id = id,
                Nombre = nombre
            };
        }
    }
}