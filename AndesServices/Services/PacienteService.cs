using System.Net.Http.Headers;
using AndesServices.DTOs;
using AndesServices.Entities;
using AndesServices.Interfaces;
using Newtonsoft.Json;

namespace AndesServices.Services
{
    public class PacienteService : IPaciente
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PacienteService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Paciente> ObtenerPacientePorIdAsync(string idPaciente, string? token = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                }

                using (HttpResponseMessage res = await client.GetAsync($"modules/mobileApp/paciente/{idPaciente}"))
                {
                    res.EnsureSuccessStatusCode();

                    Paciente unPaciente = await res.Content.ReadFromJsonAsync<Paciente>();

                    if (unPaciente == null)
                    {
                        Console.WriteLine("No se encontró el paciente.");
                        return null;
                    }

                    return unPaciente;
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener los datos del paciente: {exception.Message}");
                return null;
            }
        }

        public async Task<userLocation> ObtenerGeoreferenciaPaciente(string direccion)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Andes-NoJWT");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"modules/georeferencia/georeferenciar?direccion={Uri.EscapeDataString(direccion)}", UriKind.Relative),
                };

                using (HttpResponseMessage res = await client.SendAsync(request))
                {
                    res.EnsureSuccessStatusCode();

                    userLocation unaGeoreferencia = await res.Content.ReadFromJsonAsync<userLocation>();

                    if (unaGeoreferencia == null)
                    {
                        Console.WriteLine("No se encontró la georeferencia del paciente.");
                        return null;
                    }

                    return unaGeoreferencia;
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener los datos del paciente: {exception.Message}");
                return null;
            }
        }

        // Modifica datos personales de un Paciente
        public async Task<Paciente> ModificarDatos(string idPaciente, ActualizarPacienteDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // Validar género si está presente
            if (!string.IsNullOrEmpty(dto.Genero))
            {
                var generosValidos = new List<string> { "mujer", "mujer trans", "varon", "varon trans", "no binario", "travesti", "masculinidad trans", "femenino", "masculino", "otro" };
                if (!generosValidos.Contains(dto.Genero))
                {
                    throw new ArgumentException("Genero no permitido.");
                }
            }
            
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("Andes");

                // Obtener el paciente original del servidor para comparar cambios
                Paciente pacienteOriginal = await ObtenerPacientePorIdAsync(idPaciente);
                if (pacienteOriginal == null)
                {
                    throw new Exception("No se pudo obtener el paciente original del servidor.");
                }

                // Obtener fecha/hora actual para ultimaActualizacion
                DateTime fechaActual = DateTime.UtcNow;

                // Actualizar ultimaActualizacion en contactos
                if (dto.Contacto != null)
                {
                    foreach (var c in dto.Contacto)
                    {
                        var contactoOriginal = pacienteOriginal?.contacto?.FirstOrDefault(co =>
                            co.id == c.Id ||
                            co._id == c.IdInterno ||
                            co.id == c.IdInterno ||
                            co._id == c.Id);

                        if (contactoOriginal == null ||
                            contactoOriginal.valor != c.Valor ||
                            contactoOriginal.tipo != c.Tipo)
                        {
                            c.UltimaActualizacion = fechaActual;
                        }
                    }
                }

                // Actualizar ultimaActualizacion en direcciones y obtener georeferencia
                if (dto.Direccion != null)
                {
                    foreach (var d in dto.Direccion)
                    {
                        var direccionOriginal = pacienteOriginal?.direccion?.FirstOrDefault(di =>
                            di.id == d.Id ||
                            di._id == d.IdInterno ||
                            di.id == d.IdInterno ||
                            di._id == d.Id);

                        if (direccionOriginal == null ||
                            direccionOriginal.valor != d.Valor ||
                            direccionOriginal.codigoPostal != d.CodigoPostal ||
                            direccionOriginal.ranking != d.Ranking ||
                            direccionOriginal.ubicacion?.pais?.nombre != d.Ubicacion?.Pais?.Nombre ||
                            direccionOriginal.ubicacion?.provincia?.nombre != d.Ubicacion?.Provincia?.Nombre ||
                            direccionOriginal.ubicacion?.localidad?.nombre != d.Ubicacion?.Localidad?.Nombre)
                        {
                            d.UltimaActualizacion = fechaActual;
                        }
                    }
                }

                // Serializar el DTO de actualización
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"modules/mobileApp/pacientes/{idPaciente}", UriKind.Relative),
                    Content = new StringContent(JsonConvert.SerializeObject(dto), System.Text.Encoding.UTF8, "application/json")
                };

                using (HttpResponseMessage res = await client.SendAsync(request))
                {
                    res.EnsureSuccessStatusCode();

                    Paciente pacienteActualizado = await res.Content.ReadFromJsonAsync<Paciente>();

                    if (pacienteActualizado == null)
                    {
                        throw new Exception("No se pudo obtener la respuesta del servidor.");
                    }

                    return pacienteActualizado;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al modificar los datos del paciente: {exception.Message}");
                throw;
            }
        }

        public Direccion? ObtenerDireccionPrioritaria(Paciente? paciente)
        {
            List<Direccion>? direccion = paciente?.direccion;

            if (direccion?.Count == 1)
            {
                return direccion[0];
            }
            if (direccion?.Count > 1)
            {
                return direccion[1];
            }
            return null;
        }

        public ActualizarPacienteDireccionDto? ObtenerDireccionPrioritaria(ActualizarPacienteDto pacienteActualizado)
        {
            if (pacienteActualizado.Direccion.Count == 1)
            {
                return pacienteActualizado.Direccion[0];
            }

            if (pacienteActualizado.Direccion.Count > 1)
            {
                return pacienteActualizado.Direccion[1];
            }

            return null;
        }
    }
}
