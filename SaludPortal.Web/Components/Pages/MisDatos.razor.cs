using AndesServices.Entities;
using AndesServices.Interfaces;
using BlazorSpinner;
using Newtonsoft.Json;
using SaludPortal.Web.Components.Shared;

namespace SaludPortal.Web.Components.Pages;

public partial class MisDatos : AuthenticatedComponentBase
{
    private Paciente? _paciente = null;
    private Paciente? _pacienteOriginal = null;
    private Direccion? _direccionSeleccionada = null;
    private string? _provinciaSeleccionadaId = null;
    private string? _localidadSeleccionadaId = null;
    private List<Provincia> _provincias = new List<Provincia>();
    private List<Localidad>? _localidades = null;
    private string? _errorProvincia = null;
    private string? _errorLocalidad = null;
    private string? _errorCodigoPostal = null;
    private bool _cargandoLocalidades = false;
    
    private readonly List<string> _generosPermitidos = new List<string>
    {
        "mujer",
        "mujer trans",
        "varon",
        "varon trans",
        "no binario",
        "travesti",
        "masculinidad trans",
        "femenino",
        "masculino",
        "otro"
    };

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        var token = BackendToken;
        if (string.IsNullOrEmpty(token))
            return;

        var pacienteId = PacienteId;
        if (string.IsNullOrEmpty(pacienteId))
            return;

        _spinnerService.Show();

        try
        {
            // Cargar provincias desde el servicio
            _provincias = await _territorioService.ObtenerProvinciasAsync(token);

            // Obtengo el paciente
            _paciente = await _pacienteService.ObtenerPacientePorIdAsync(token, pacienteId);

            // Guardar copia del paciente original para comparación
            ActualizarPacienteOriginal(_paciente);

            await MostrarYActualizarPaciente(_paciente);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener los datos del paciente: {ex.Message}");
        }
        finally
        {
            _spinnerService.Hide();
        }
    }

    private async Task MostrarYActualizarPaciente(Paciente? paciente)
    {
        var json = JsonConvert.SerializeObject(paciente);
        _paciente = JsonConvert.DeserializeObject<Paciente>(json);

        // Verifico que domicilio utilizar
        if (_paciente != null)
        {
            if (_paciente?.direccion?.Count() > 1)
            {
                _direccionSeleccionada = _paciente?.direccion[1];
            }
            else
            {
                if (_paciente?.direccion?.Count() == 1) 
                    _direccionSeleccionada = _paciente?.direccion[0];
            }
            
            // Inicializar provincia seleccionada si existe
            if (_direccionSeleccionada?.ubicacion?.provincia != null)
            {
                // Buscar la provincia en la lista cargada por id o nombre
                var provinciaExistente = _provincias.FirstOrDefault(p => 
                    p.id == _direccionSeleccionada.ubicacion.provincia.id ||
                    p._id == _direccionSeleccionada.ubicacion.provincia._id ||
                    p.nombre == _direccionSeleccionada.ubicacion.provincia.nombre);
                
                if (provinciaExistente != null)
                {
                    _provinciaSeleccionadaId = provinciaExistente.id ?? provinciaExistente._id;
                    
                    // Actualizar la provincia con todos los IDs desde la lista cargada
                    _direccionSeleccionada.ubicacion.provincia.id = provinciaExistente.id;
                    _direccionSeleccionada.ubicacion.provincia._id = provinciaExistente._id;
                    _direccionSeleccionada.ubicacion.provincia.nombre = provinciaExistente.nombre;
                    
                    // Cargar localidades de la provincia seleccionada
                    await CargarLocalidadesAsync(_provinciaSeleccionadaId);
                    
                    // Inicializar localidad seleccionada si existe
                    if (_direccionSeleccionada.ubicacion.localidad != null && _localidades != null)
                    {
                        var localidadExistente = _localidades.FirstOrDefault(l =>
                            l.id == _direccionSeleccionada.ubicacion.localidad.id ||
                            l._id == _direccionSeleccionada.ubicacion.localidad._id ||
                            l.nombre == _direccionSeleccionada.ubicacion.localidad.nombre);
                        
                        if (localidadExistente != null)
                        {
                            _localidadSeleccionadaId = localidadExistente.id ?? localidadExistente._id;
                            
                            // Actualizar la localidad con todos los IDs desde la lista cargada
                            _direccionSeleccionada.ubicacion.localidad.id = localidadExistente.id;
                            _direccionSeleccionada.ubicacion.localidad._id = localidadExistente._id;
                            _direccionSeleccionada.ubicacion.localidad.nombre = localidadExistente.nombre;
                            _direccionSeleccionada.ubicacion.localidad.localidadId = localidadExistente.localidadId;
                        }
                    }
                }
            }
            else if (_direccionSeleccionada?.ubicacion != null)
            {
                _direccionSeleccionada.ubicacion.provincia = new Provincia();
            }
        }
    }

    private async Task OnProvinciaChanged()
    {
        // Limpiar mensaje de error de provincia
        _errorProvincia = null;
        
        // Limpiar localidad seleccionada cuando cambia la provincia
        _localidadSeleccionadaId = null;
        _localidades = null;
        _errorLocalidad = null;
        
        if (string.IsNullOrEmpty(_provinciaSeleccionadaId))
        {
            if (_direccionSeleccionada?.ubicacion != null)
            {
                _direccionSeleccionada.ubicacion.provincia = null;
                _direccionSeleccionada.ubicacion.localidad = null;
            }
            return;
        }

        // Cargar localidades de la provincia seleccionada
        await CargarLocalidadesAsync(_provinciaSeleccionadaId);

        // Actualizar la provincia en la dirección
        if (_direccionSeleccionada?.ubicacion != null)
        {
            var provinciaSeleccionada = _provincias.FirstOrDefault(p => 
                p.id == _provinciaSeleccionadaId || p._id == _provinciaSeleccionadaId);
            
            if (provinciaSeleccionada != null)
            {
                _direccionSeleccionada.ubicacion.provincia = new Provincia
                {
                    id = provinciaSeleccionada.id,
                    _id = provinciaSeleccionada._id,
                    nombre = provinciaSeleccionada.nombre
                };
            }
            
            // Limpiar localidad cuando cambia la provincia
            _direccionSeleccionada.ubicacion.localidad = null;
        }
    }

    private async Task OnLocalidadChanged()
    {
        // Limpiar mensaje de error de localidad
        _errorLocalidad = null;
        
        if (string.IsNullOrEmpty(_localidadSeleccionadaId) || _localidades == null)
        {
            if (_direccionSeleccionada?.ubicacion != null)
            {
                _direccionSeleccionada.ubicacion.localidad = null;
            }
            return;
        }

        // Actualizar la localidad en la dirección
        if (_direccionSeleccionada?.ubicacion != null)
        {
            var localidadSeleccionada = _localidades.FirstOrDefault(l => 
                l.id == _localidadSeleccionadaId || l._id == _localidadSeleccionadaId);
            
            if (localidadSeleccionada != null)
            {
                _direccionSeleccionada.ubicacion.localidad = new Localidad
                {
                    id = localidadSeleccionada.id,
                    _id = localidadSeleccionada._id,
                    nombre = localidadSeleccionada.nombre,
                    localidadId = localidadSeleccionada.localidadId
                };
            }
        }
    }

    private bool ValidarCodigoPostal()
    {
        _errorCodigoPostal = null;
        
        if (_direccionSeleccionada == null)
        {
            return true;
        }
        
        // Validar que no esté vacío
        if (string.IsNullOrWhiteSpace(_direccionSeleccionada.codigoPostal))
        {
            _errorCodigoPostal = "El código postal es obligatorio.";
            return false;
        }
        
        // Validar formato: debe tener 4 dígitos numéricos
        string codigoPostal = _direccionSeleccionada.codigoPostal.Trim();
        if (codigoPostal.Length != 4 || !codigoPostal.All(char.IsDigit))
        {
            _errorCodigoPostal = "El código postal debe tener 4 dígitos numéricos.";
            return false;
        }
        
        return true;
    }

    private void OnCodigoPostalChanged()
    {
        ValidarCodigoPostal();
    }

    private async Task CargarLocalidadesAsync(string? idProvincia)
    {
        if (string.IsNullOrEmpty(idProvincia))
        {
            _localidades = null;
            _cargandoLocalidades = false;
            return;
        }

        var token = BackendToken;
        if (string.IsNullOrEmpty(token))
        {
            _localidades = new List<Localidad>();
            _cargandoLocalidades = false;
            return;
        }

        _cargandoLocalidades = true;
        StateHasChanged();

        try
        {
            _localidades = await _territorioService.ObtenerLocalidadesPorProvinciaAsync(token, idProvincia);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar localidades: {ex.Message}");
            _localidades = new List<Localidad>();
        }
        finally
        {
            _cargandoLocalidades = false;
            StateHasChanged();
        }
    }

    private async Task OnActualizarDatosClick()
    {
        if (_paciente == null)
        {
            Console.WriteLine("Error: No hay datos del paciente para actualizar.");
            return;
        }

        if (_direccionSeleccionada == null && _paciente.direccion != null && _paciente.direccion.Any())
        {
            Console.WriteLine("Error: No hay domicilio seleccionado para actualizar.");
            return;
        }

        // Validar que se haya seleccionado una provincia
        if (string.IsNullOrEmpty(_provinciaSeleccionadaId))
        {
            _errorProvincia = "Debe seleccionar una provincia.";
            StateHasChanged();
            return;
        }
        else
        {
            _errorProvincia = null;
        }

        // Validar que se haya seleccionado una localidad
        if (string.IsNullOrEmpty(_localidadSeleccionadaId))
        {
            _errorLocalidad = "Debe seleccionar una localidad.";
            StateHasChanged();
            return;
        }
        else
        {
            _errorLocalidad = null;
        }

        // Validar código postal si está presente
        if (!ValidarCodigoPostal())
        {
            StateHasChanged();
            return;
        }

        var token = BackendToken;
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Error: Token no disponible.");
            return;
        }

        var pacienteId = PacienteId;
        if (string.IsNullOrEmpty(pacienteId))
        {
            Console.WriteLine("Error: ID de paciente no disponible.");
            return;
        }

        _spinnerService.Show();

        try
        {
            // Sincronizar el domicilio modificado en _direccionSeleccionada con _paciente.direccion
            if (_direccionSeleccionada != null && _paciente.direccion != null)
            {
                var direccionEnPaciente = _paciente.direccion.FirstOrDefault(d => 
                    (d.id != null && d.id == _direccionSeleccionada.id) || 
                    (d._id != null && d._id == _direccionSeleccionada._id));

                if (direccionEnPaciente != null)
                {
                    // Actualizar la dirección en el array del paciente con los cambios de _direccionSeleccionada
                    direccionEnPaciente.valor = _direccionSeleccionada.valor;
                    direccionEnPaciente.codigoPostal = _direccionSeleccionada.codigoPostal;
                    direccionEnPaciente.ranking = _direccionSeleccionada.ranking;
                    direccionEnPaciente.activo = _direccionSeleccionada.activo;
                    direccionEnPaciente.geoReferencia = _direccionSeleccionada.geoReferencia;
                    
                    if (_direccionSeleccionada.ubicacion != null)
                    {
                        if (direccionEnPaciente.ubicacion == null)
                        {
                            direccionEnPaciente.ubicacion = new Ubicacion();
                        }
                        
                        if (_direccionSeleccionada.ubicacion.pais != null)
                        {
                            if (direccionEnPaciente.ubicacion.pais == null)
                            {
                                direccionEnPaciente.ubicacion.pais = new Pais();
                            }
                            direccionEnPaciente.ubicacion.pais.nombre = _direccionSeleccionada.ubicacion.pais.nombre;
                        }
                        
                        if (_direccionSeleccionada.ubicacion.provincia != null)
                        {
                            if (direccionEnPaciente.ubicacion.provincia == null)
                            {
                                direccionEnPaciente.ubicacion.provincia = new Provincia();
                            }
                            direccionEnPaciente.ubicacion.provincia.id = _direccionSeleccionada.ubicacion.provincia.id;
                            direccionEnPaciente.ubicacion.provincia._id = _direccionSeleccionada.ubicacion.provincia._id;
                            direccionEnPaciente.ubicacion.provincia.nombre = _direccionSeleccionada.ubicacion.provincia.nombre;
                        }
                        
                        if (_direccionSeleccionada.ubicacion.localidad != null)
                        {
                            if (direccionEnPaciente.ubicacion.localidad == null)
                            {
                                direccionEnPaciente.ubicacion.localidad = new Localidad();
                            }
                            direccionEnPaciente.ubicacion.localidad.id = _direccionSeleccionada.ubicacion.localidad.id;
                            direccionEnPaciente.ubicacion.localidad._id = _direccionSeleccionada.ubicacion.localidad._id;
                            direccionEnPaciente.ubicacion.localidad.nombre = _direccionSeleccionada.ubicacion.localidad.nombre;
                            direccionEnPaciente.ubicacion.localidad.localidadId = _direccionSeleccionada.ubicacion.localidad.localidadId;
                        }
                        
                        direccionEnPaciente.ubicacion.barrio = null;
                    }
                }
            }

            // Llamar al servicio para actualizar
            var pacienteActualizado = await _pacienteService.ModificarDatos(token, pacienteId, _paciente);

            if (pacienteActualizado != null)
            {
                // Actualizar el paciente con la respuesta del servidor
                _paciente = pacienteActualizado;

                // Actualizar _direccionSeleccionada con el domicilio actualizado de la respuesta
                if (_direccionSeleccionada != null && _paciente.direccion != null)
                {
                    var direccionActualizada = _paciente.direccion.FirstOrDefault(d => 
                        (d.id != null && d.id == _direccionSeleccionada.id) || 
                        (d._id != null && d._id == _direccionSeleccionada._id));

                    if (direccionActualizada != null)
                    {
                        _direccionSeleccionada = direccionActualizada;
                        
                        // Actualizar provincia seleccionada
                        if (_direccionSeleccionada.ubicacion?.provincia != null)
                        {
                            var provinciaExistente = _provincias.FirstOrDefault(p => 
                                p.id == _direccionSeleccionada.ubicacion.provincia.id ||
                                p._id == _direccionSeleccionada.ubicacion.provincia._id ||
                                p.nombre == _direccionSeleccionada.ubicacion.provincia.nombre);
                            
                            if (provinciaExistente != null)
                            {
                                _provinciaSeleccionadaId = provinciaExistente.id ?? provinciaExistente._id;
                                
                                // Asegurar que los IDs de la provincia estén actualizados
                                _direccionSeleccionada.ubicacion.provincia.id = provinciaExistente.id;
                                _direccionSeleccionada.ubicacion.provincia._id = provinciaExistente._id;
                                _direccionSeleccionada.ubicacion.provincia.nombre = provinciaExistente.nombre;
                                
                                await CargarLocalidadesAsync(_provinciaSeleccionadaId);
                                
                                // Actualizar localidad seleccionada
                                if (_direccionSeleccionada.ubicacion.localidad != null && _localidades != null)
                                {
                                    var localidadExistente = _localidades.FirstOrDefault(l =>
                                        l.id == _direccionSeleccionada.ubicacion.localidad.id ||
                                        l._id == _direccionSeleccionada.ubicacion.localidad._id ||
                                        l.nombre == _direccionSeleccionada.ubicacion.localidad.nombre);
                                    
                                    if (localidadExistente != null)
                                    {
                                        _localidadSeleccionadaId = localidadExistente.id ?? localidadExistente._id;
                                        
                                        // Asegurar que los IDs de la localidad estén actualizados
                                        _direccionSeleccionada.ubicacion.localidad.id = localidadExistente.id;
                                        _direccionSeleccionada.ubicacion.localidad._id = localidadExistente._id;
                                        _direccionSeleccionada.ubicacion.localidad.nombre = localidadExistente.nombre;
                                        _direccionSeleccionada.ubicacion.localidad.localidadId = localidadExistente.localidadId;
                                    }
                                }
                            }
                        }
                    }
                }

                // Actualizar la copia original para futuras comparaciones
                ActualizarPacienteOriginal(_paciente);
            }
            else
            {
                Console.WriteLine("Error: No se recibió respuesta del servidor.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar los datos del paciente: {ex.Message}");
        }
        finally
        {
            _spinnerService.Hide();
        }
    }

    private void ActualizarPacienteOriginal(Paciente? unPaciente)
    {
        if (unPaciente != null)
        {
            var json = JsonConvert.SerializeObject(unPaciente);
            _pacienteOriginal = JsonConvert.DeserializeObject<Paciente>(json);
        }
    }

    private async Task OnCancelarClick()
    {
        _spinnerService.Show();
        await MostrarYActualizarPaciente(_pacienteOriginal);
        _spinnerService.Hide();
    }
}
