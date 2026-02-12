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

        public async Task<Paciente> ObtenerPacientePorIdAsync(string token, string idPaciente)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token no proporcionado.");
            }
            try
            {
                var client = _httpClientFactory.CreateClient("Andes");
                client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                using (HttpResponseMessage res = await client.GetAsync($"modules/mobileApp/paciente/{idPaciente}"))
                {
                    res.EnsureSuccessStatusCode();

                    Paciente unPaciente = await res.Content.ReadFromJsonAsync<Paciente>();

                    if (unPaciente == null)
                    {
                        Console.WriteLine("No se encontraron recetas disponibles.");
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
                var client = _httpClientFactory.CreateClient("Andes");
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
                        Console.WriteLine("No se encontraron recetas disponibles.");
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
        public async Task<Paciente> ModificarDatos(string token, string idPaciente, Paciente paciente)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token no proporcionado.");
            }

            if (paciente == null)
            {
                throw new ArgumentNullException(nameof(paciente));
            }

            // Validar género si está presente
            if (!string.IsNullOrEmpty(paciente.genero))
            {
                var generosValidos = new List<string> { "mujer", "mujer trans", "varon", "varon trans", "no binario", "travesti", "masculinidad trans", "femenino", "masculino", "otro" };
                if (!generosValidos.Contains(paciente.genero))
                {
                    throw new ArgumentException("Genero no permitido.");
                }
            }
            
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("Andes");
                client.DefaultRequestHeaders.Add("Authorization", "JWT " + token);

                // Obtener el paciente original del servidor para comparar cambios
                Paciente pacienteOriginal = await ObtenerPacientePorIdAsync(token, idPaciente);
                if (pacienteOriginal == null)
                {
                    throw new Exception("No se pudo obtener el paciente original del servidor.");
                }

                // Obtener fecha/hora actual para ultimaActualizacion
                DateTime fechaActual = DateTime.UtcNow;

                // Actualizar ultimaActualizacion en contactos
                if (paciente.contacto != null)
                {
                    foreach (var c in paciente.contacto)
                    {
                        var contactoOriginal = pacienteOriginal?.contacto?.FirstOrDefault(co => co.id == c.id);
                        if (contactoOriginal != null)
                        {
                            if (contactoOriginal.valor != c.valor || contactoOriginal.tipo != c.tipo)
                            {
                                c.ultimaActualizacion = fechaActual;
                            }
                        }
                    }
                }

                // Actualizar ultimaActualizacion en direcciones y obtener georeferencia
                if (paciente.direccion != null)
                {
                    foreach (var d in paciente.direccion)
                    {
                        var direccionOriginal = pacienteOriginal?.direccion?.FirstOrDefault(di => di.id == d.id);
                        if (direccionOriginal != null)
                        {
                            if (direccionOriginal.valor != d.valor ||
                                direccionOriginal.codigoPostal != d.codigoPostal ||
                                direccionOriginal.ranking != d.ranking ||
                                direccionOriginal.ubicacion?.pais?.nombre != d.ubicacion?.pais?.nombre ||
                                direccionOriginal.ubicacion?.provincia?.nombre != d.ubicacion?.provincia?.nombre ||
                                direccionOriginal.ubicacion?.localidad?.nombre != d.ubicacion?.localidad?.nombre)
                            {
                                d.ultimaActualizacion = fechaActual;

                                // Obtener georeferencia de la dirección actualizada
                                if (!string.IsNullOrEmpty(d.valor) && 
                                    d.ubicacion?.localidad?.nombre != null)
                                {
                                    string direccionCompleta = $"{d.valor}, {d.ubicacion.localidad.nombre}";
                                    userLocation georeferencia = await ObtenerGeoreferenciaPaciente(direccionCompleta);
                                        
                                    if (georeferencia != null)
                                    {
                                        d.geoReferencia = new List<double> { georeferencia.lat, georeferencia.lng };
                                    }
                                }
                            }
                        }
                    }
                }

                // Serializar el objeto paciente completo
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri($"modules/mobileApp/pacientes/{idPaciente}", UriKind.Relative),
                    Content = new StringContent(JsonConvert.SerializeObject(paciente), System.Text.Encoding.UTF8, "application/json")
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
    }
}
