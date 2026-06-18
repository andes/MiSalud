using AndesServices.Entities;
using SaludPortal.Application.Models.Paciente;

namespace SaludPortal.Application.Mappers;

public static class PacienteMapper
{
    public static PacienteModel MapToPacienteModel(this Paciente paciente)
    {
        return new PacienteModel
        {
            Id = paciente.id,
            Nombre = paciente.nombre,
            Apellido = paciente.apellido,
            Alias = paciente.alias,
            NombreCompleto = paciente.nombreCompleto,
            Documento = paciente.documento,
            NumeroIdentificacion = paciente.numeroIdentificacion,
            TipoIdentificacion = paciente.tipoIdentificacion,
            Sexo = paciente.sexo,
            Genero = paciente.genero,
            EstadoCivil = paciente.estadoCivil,
            FechaNacimiento = paciente.fechaNacimiento,
            FechaFallecimiento = paciente.fechaFallecimiento,
            Edad = paciente.edad,
            EdadReal = MapEdadReal(paciente.edadReal),
            Telefono = paciente.telefono,
            Cuil = paciente.cuil,
            FotoId = paciente.fotoId,
            Estado = paciente.estado,
            Activo = paciente.activo,
            CreadoEn = paciente.createdAt,
            ActualizadoEn = paciente.updatedAt,
            AgregadoEn = paciente.addedAt,
            NombreObraSocial = paciente.obraSocial?.nombre,
            LugarNacimiento = MapLugarNacimiento(paciente.lugarNacimiento),
            Contactos = paciente.contacto?.Select(MapContacto).ToList(),
            Direcciones = paciente.direccion?.Select(MapDireccion).ToList(),
            Identificadores = paciente.identificadores?.Select(MapIdentificador).ToList(),
            Financiadores = paciente.financiador?.Select(MapFinanciador).ToList(),
            Documentos = paciente.documentos?.Select(MapDocumentoAdjunto).ToList(),
            Relaciones = paciente.relaciones?.Select(MapRelacion).ToList(),
            Vinculos = paciente.vinculos
        };
    }

    private static EdadRealModel? MapEdadReal(EdadReal? edadReal)
    {
        if (edadReal == null) return null;
        return new EdadRealModel
        {
            Valor = edadReal.valor,
            Unidad = edadReal.unidad
        };
    }

    private static LugarNacimientoModel? MapLugarNacimiento(LugarNacimiento? lugar)
    {
        if (lugar == null) return null;
        return new LugarNacimientoModel
        {
            Pais = MapReferencia(lugar.pais?._id, lugar.pais?.nombre)
        };
    }

    private static ContactoModel MapContacto(Contacto contacto)
    {
        return new ContactoModel
        {
            Activo = contacto.activo,
            UltimaActualizacion = contacto.ultimaActualizacion,
            Ranking = contacto.ranking,
            Valor = contacto.valor,
            Tipo = contacto.tipo
        };
    }

    private static DireccionModel MapDireccion(Direccion direccion)
    {
        return new DireccionModel
        {
            GeoReferencia = direccion.geoReferencia,
            Activo = direccion.activo,
            UltimaActualizacion = direccion.ultimaActualizacion,
            Ranking = direccion.ranking,
            CodigoPostal = direccion.codigoPostal,
            Valor = direccion.valor,
            Ubicacion = MapUbicacion(direccion.ubicacion)
        };
    }

    private static UbicacionModel? MapUbicacion(Ubicacion? ubicacion)
    {
        if (ubicacion == null) return null;
        return new UbicacionModel
        {
            Pais = MapReferencia(ubicacion.pais?._id, ubicacion.pais?.nombre),
            Provincia = MapReferencia(ubicacion.provincia?._id, ubicacion.provincia?.nombre),
            Localidad = MapReferencia(ubicacion.localidad?._id, ubicacion.localidad?.nombre),
            Barrio = MapReferencia(ubicacion.barrio?._id, ubicacion.barrio?.nombre)
        };
    }

    private static IdentificadorModel MapIdentificador(Identificador identificador)
    {
        return new IdentificadorModel
        {
            Entidad = identificador.entidad,
            Valor = identificador.valor
        };
    }

    private static FinanciadorModel MapFinanciador(Financiador financiador)
    {
        return new FinanciadorModel
        {
            CodigoPuco = financiador.codigoPuco,
            Nombre = financiador.nombre,
            Origen = financiador.origen,
            FechaDeActualizacion = financiador.fechaDeActualizacion,
            Prepaga = financiador.prepaga
        };
    }

    private static DocumentoAdjuntoModel MapDocumentoAdjunto(DocumentoAdjunto documento)
    {
        return new DocumentoAdjuntoModel
        {
            Tipo = documento.tipo == null ? null : new TipoDocumentoModel
            {
                Id = documento.tipo.id,
                Label = documento.tipo.label
            },
            Archivos = documento.archivos?.Select(a => new ArchivoAdjuntoModel { Ext = a.ext }).ToList(),
            Fecha = documento.fecha
        };
    }

    private static RelacionModel MapRelacion(Relacion relacion)
    {
        return new RelacionModel
        {
            Relacion = relacion.relacion == null ? null : new RelacionTipoModel
            {
                Nombre = relacion.relacion.nombre,
                Opuesto = relacion.relacion.opuesto,
                EsConviviente = relacion.relacion.esConviviente
            },
            Nombre = relacion.nombre,
            Apellido = relacion.apellido,
            Documento = relacion.documento,
            FechaNacimiento = relacion.fechaNacimiento,
            FechaFallecimiento = relacion.fechaFallecimiento,
            Activo = relacion.activo
        };
    }

    private static ReferenciaModel? MapReferencia(string? id, string? nombre)
    {
        if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(nombre)) return null;
        return new ReferenciaModel { Id = id, Nombre = nombre };
    }
}
