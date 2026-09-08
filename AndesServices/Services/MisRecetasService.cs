using AndesServices.Entities;
using AndesServices.Interfaces;

namespace AndesServices.Services
{
    public class MisRecetasService : IMisRecetas
    {
        private readonly ILogger<MisRecetasService> _logger;
        private readonly HttpClient _andesClient;

        public MisRecetasService(IHttpClientFactory httpClientFactory, ILogger<MisRecetasService> logger)
        {
            _logger = logger;
            _andesClient = httpClientFactory.CreateClient("Andes");
        }

        public Task<bool> ActualizarEstadoRecetaAsync(string recetaId, Estado nuevoEstado)
        {
            throw new NotImplementedException();
        }

        public Task<MisReceta> ObtenerRecetaPorIdAsync(string recetaId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MisReceta>> ObtenerRecetasPacienteAsync(string pacienteId)
        {
            try
            {
                string estado = "sin-dispensa,dispensada,dispensa-parcial";
                //string queryParams = "?estado=" + estado + "&dni=" + dni + "&fecNac=" + fecNac + "&apellido=" + apellido + "&fechaDde=" + fechaDde + "&fechaHta=" + fechaHta;
                string url = $"modules/recetas?pacienteId={pacienteId}&estadoDispensa={estado}";

                using (HttpResponseMessage res = await _andesClient.GetAsync(url))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        return await res.Content.ReadFromJsonAsync<List<MisReceta>>();
                    }
                    else
                    {
                        string body = await res.Content.ReadAsStringAsync();
                        _logger.LogError("Obtener recetas fallo HTTP {Code} {Reason} Body:{Body}", (int)res.StatusCode, res.ReasonPhrase, body);
                        return null;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error al obtener las recetas para el paciente {PacienteId}", pacienteId);
                return null;
            }
            return null;
        }
    }
}
