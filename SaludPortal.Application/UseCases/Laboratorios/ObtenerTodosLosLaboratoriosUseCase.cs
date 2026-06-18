using AndesServices.Interfaces;
using SaludPortal.Application.Models.Laboratorios;
using SaludPortal.Application.Utils;

namespace SaludPortal.Application.UseCases.Laboratorios;

public class ObtenerTodosLosLaboratoriosUseCase
{
    private readonly IMisLaboratorios _laboratoriosService;

    public ObtenerTodosLosLaboratoriosUseCase(IMisLaboratorios laboratoriosService)
    {
        _laboratoriosService = laboratoriosService;
    }

    public async Task<List<LaboratorioModel>> EjecutarAsync(string pacienteId, string documento, string fechaDesde)
    {
        var fechaActual = DateTimeHelper.NowArgentina().ToString("yyyyMMdd");
        var todos = new List<LaboratorioModel>();

        // Labs estándar de Andes
        var labsAndes = await _laboratoriosService.ObtenerMisLaboratoriosAsync(pacienteId, fechaDesde, fechaActual);
        if (labsAndes != null)
        {
            todos.AddRange(labsAndes.Select(l => new LaboratorioModel
            {
                IdProtocolo = l.idProtocolo,
                Documento = l.documento,
                Apellido = l.apellido,
                Nombre = l.nombre,
                Fecha = l.fecha,
                Laboratorio = l.Laboratorio,
                MedicoSolicitante = l.medicoSolicitante,
                Tipo = l.tipo
            }));
        }

        // Labs CDA (filtrados por SNOMED de laboratorio)
        var labsCDA = await _laboratoriosService.ObtenerMisLaboratoriosCDAAsync(pacienteId, fechaDesde, fechaActual);
        if (labsCDA != null)
        {
            foreach (var cda in labsCDA.Where(c => c?.prestacion?.snomed?.conceptId == "4241000179101"))
            {
                todos.Add(new LaboratorioModel
                {
                    IdProtocolo = cda.cda_id,
                    Documento = "",
                    Apellido = "",
                    Nombre = "",
                    Fecha = DateTimeHelper.ToArgentinaTime(cda.fecha).ToString("dd/MM/yyyy"),
                    Laboratorio = cda.organizacion.nombre,
                    MedicoSolicitante = $"{cda.profesional.apellido} {cda.profesional.nombre}",
                    Tipo = "cda",
                    CdaAdjuntos = cda.adjuntos
                });
            }
        }

        // Labs Rania
        var protocolosRania = await _laboratoriosService.ObtenerProtocolosRania(documento);
        if (protocolosRania != null)
        {
            foreach (var protocolo in protocolosRania)
            {
                todos.Add(new LaboratorioModel
                {
                    IdProtocolo = protocolo.ProtocoloId,
                    Documento = documento,
                    Apellido = "",
                    Nombre = "",
                    Fecha = protocolo.Fecha,
                    Laboratorio = "Clínica Dr. Roberto Raña",
                    MedicoSolicitante = $"{protocolo.SolicitanteApellido} {protocolo.SolicitanteNombre}",
                    Tipo = "raña"
                });
            }
        }

        // Ordenar por fecha descendente
        return todos.OrderByDescending(l =>
        {
            if (string.IsNullOrEmpty(l.Fecha))
                return DateTime.MinValue;

            if (DateTime.TryParseExact(l.Fecha, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }

            return DateTime.MinValue;
        }).ToList();
    }
}
