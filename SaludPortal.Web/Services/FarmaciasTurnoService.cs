using AndesServices.Entities;
using AndesServices.Interfaces;
using Newtonsoft.Json.Linq;

namespace SaludPortal.Web.Services
{
    public class FarmaciasTurnoService
    {
        private readonly IConfiguration _configuration;
        public FarmaciasTurnoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<FarmaciasTurno>> ObtenerFarmaciasTurnoAsync(string localidadId, string fechaDesde, string fechaHasta)
        {

            if (string.IsNullOrEmpty(localidadId))
            {
                Console.WriteLine("Se debe seleccionar una localidad.");
                return null;
            }
            try
            {
                AndesServices.Services.FarmaciasTurnoService farmaciasTurno = new AndesServices.Services.FarmaciasTurnoService(_configuration);
                return await farmaciasTurno.ObtenerFarmaciasTurnoAsync(localidadId,fechaDesde,fechaHasta);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener las farmacias.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }

        public async Task<List<Localidad>> ObtenerLocalidadesAsync()
        {
            try
            {
                AndesServices.Services.FarmaciasTurnoService farmaciasTurno = new AndesServices.Services.FarmaciasTurnoService(_configuration);
                return await farmaciasTurno.ObtenerLocalidadesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Se produjo un error al obtener las localidades.");
                Console.WriteLine(exception.Message);
            }
            return null;
        }
    }
}
