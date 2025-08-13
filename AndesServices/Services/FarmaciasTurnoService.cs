using AndesServices.Entities;
using AndesServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace AndesServices.Services
{
    public class FarmaciasTurnoService : IFarmaciasTurno
    {
        private readonly IConfiguration _configuration;

        public FarmaciasTurnoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<FarmaciasTurno>> ObtenerFarmaciasTurnoAsync(string localidadId, string fechaDesde, string fechaHasta)
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/modules/mobileApp/farmacias/turnos"
                : conexionServicios.UrlProyectoServiciosDemo + "/modules/mobileApp/farmacias/turnos";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string queryParams = "?localidad=" + localidadId + "&desde=" + fechaDesde + "&hasta=" + fechaHasta;

                    using (HttpResponseMessage res = await client.GetAsync(url + queryParams))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            List<FarmaciasTurno>? LstFarmacias = await res.Content.ReadFromJsonAsync<List<FarmaciasTurno>>();
                            if (LstFarmacias == null)
                            {
                                Console.WriteLine("No se encontraron farmacias disponibles.");
                                return null;
                            }

                            return LstFarmacias;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener las farmacias: {exception.Message}");
                return null;
            }
            return null;
        }

        public async Task<List<Localidad>> ObtenerLocalidadesAsync()
        {
            var conexionServicios = new ConexionServicios();
            _configuration.GetSection("urlServicios").Bind(conexionServicios);

            string url = conexionServicios.usarProd
                ? conexionServicios.UrlProyectoServiciosProd + "/modules/mobileApp/farmacias/localidades"
                : conexionServicios.UrlProyectoServiciosDemo + "/modules/mobileApp/farmacias/localidades";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(url))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            List<Localidad>? LstLocalidades = await res.Content.ReadFromJsonAsync<List<Localidad>>();
                            if (LstLocalidades == null)
                            {
                                Console.WriteLine("No se encontraron farmacias disponibles.");
                                return null;
                            }

                            return LstLocalidades;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error al obtener las localidades: {exception.Message}");
                return null;
            }
            return null;
        }
    }
}
