using Microsoft.AspNetCore.Components;
using SaludPortal.Web.Components.Shared;
using AndesServices;
using AndesServices.DTOs;
using AndesServices.Interfaces;
using AndesServices.Entities;
using SaludPortal.Web.Models;
using BlazorSpinner;
using System.Text.RegularExpressions;

namespace SaludPortal.Web.Components.Pages
{
    public partial class MisDatos : AuthenticatedComponentBase
    {
        [Inject] private SpinnerService _spinnerService { get; set; } = default!;
        [Inject] private IPaciente _pacienteService { get; set; } = default!;
        [Inject] private ITerritorio _territorioService { get; set; } = default!;

        private AndesServices.Entities.Paciente? paciente;
        private List<Provincia>? provincias;
        private List<Localidad>? localidades;
        private bool modoEdicion = false;
        private bool cargandoLocalidades = false;
        private string mensajeExito = string.Empty;
        private string mensajeError = string.Empty;
        private string? nombreLocalidadPaciente = null; // Para buscar la localidad por nombre después de cargarla

        private MisDatosFormModel formModel = new();

        // Variables para celular (un solo campo)
        private string celular = "";
        private bool errorCelular = false;
        private const int LONGITUD_MAXIMA_CELULAR = 10;

        // Expresiones regulares para validación
        private static readonly Regex SoloNumerosRegex = new Regex(@"[^\d]", RegexOptions.Compiled);
        private static readonly Regex IniciaConCeroRegex = new Regex(@"^0+", RegexOptions.Compiled);
        private static readonly Regex IniciaConQuinceRegex = new Regex(@"^15", RegexOptions.Compiled);

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
                // Cargar paciente
                paciente = await _pacienteService.ObtenerPacientePorIdAsync(pacienteId);

                // Cargar provincias
                provincias = await _territorioService.ObtenerProvinciasAsync();

                // Inicializar formulario con datos del paciente
                if (paciente != null)
                {
                    InicializarFormulario();

                    // Si hay provincia seleccionada, cargar localidades
                    if (!string.IsNullOrEmpty(formModel.ProvinciaId))
                    {
                        await CargarLocalidadesIniciales();
                    }
                }
            }
            catch (Exception ex)
            {
                mensajeError = $"Error al cargar los datos: {ex.Message}";
            }
            finally
            {
                _spinnerService.Hide();
            }
        }

        private void InicializarFormulario()
        {
            if (paciente == null) return;

            // Datos personales
            formModel.Alias = paciente.alias ?? string.Empty;
            formModel.Genero = paciente.genero ?? string.Empty;

            // Domicilio
            var direccionPrincipal = _pacienteService.ObtenerDireccionPrioritaria(paciente);
            if (direccionPrincipal != null)
            {
                formModel.Direccion = direccionPrincipal.valor ?? string.Empty;
                formModel.CodigoPostal = direccionPrincipal.codigoPostal ?? string.Empty;

                // Buscar provincia por nombre
                if (direccionPrincipal.ubicacion?.provincia != null && !string.IsNullOrEmpty(direccionPrincipal.ubicacion.provincia.nombre))
                {
                    var provinciaEncontrada = provincias?.FirstOrDefault(p =>
                        p.nombre?.Equals(direccionPrincipal.ubicacion.provincia.nombre, StringComparison.OrdinalIgnoreCase) == true);
                    if (provinciaEncontrada != null)
                    {
                        formModel.ProvinciaId = provinciaEncontrada.id ?? string.Empty;
                    }
                }

                // Guardar el nombre de la localidad para buscarla después cuando se carguen las localidades
                if (direccionPrincipal.ubicacion?.localidad != null && !string.IsNullOrEmpty(direccionPrincipal.ubicacion.localidad.nombre))
                {
                    nombreLocalidadPaciente = direccionPrincipal.ubicacion.localidad.nombre;
                }
            }

            // Contacto
            if (paciente.contacto != null)
            {
                var emailContacto = paciente.contacto.FirstOrDefault(c => c.tipo == "email");
                if (emailContacto != null)
                {
                    formModel.Email = emailContacto.valor ?? string.Empty;
                }

                var celularContacto = paciente.contacto.FirstOrDefault(c => c.tipo == "celular");
                if (celularContacto != null && !string.IsNullOrWhiteSpace(celularContacto.valor))
                {
                    // Normalizar: eliminar caracteres no numéricos, eliminar 0 inicial y prefijo 15
                    var celularNormalizado = SoloNumerosRegex.Replace(celularContacto.valor, "");
                    celularNormalizado = IniciaConCeroRegex.Replace(celularNormalizado, "");
                    celularNormalizado = IniciaConQuinceRegex.Replace(celularNormalizado, "");

                    celular = celularNormalizado;
                }
            }
        }

        private async Task OnProvinciaChanged()
        {
            if (string.IsNullOrEmpty(formModel.ProvinciaId))
            {
                localidades = null;
                formModel.LocalidadId = string.Empty;
                return;
            }

            cargandoLocalidades = true;
            StateHasChanged();

            try
            {
                var token = BackendToken;
                if (!string.IsNullOrEmpty(token))
                {
                    localidades = await _territorioService.ObtenerLocalidadesPorProvinciaAsync(formModel.ProvinciaId);
                    formModel.LocalidadId = string.Empty; // Reset localidad al cambiar provincia
                }
            }
            catch (Exception ex)
            {
                mensajeError = $"Error al cargar las localidades: {ex.Message}";
            }
            finally
            {
                cargandoLocalidades = false;
                StateHasChanged();
            }
        }

        private async Task CargarLocalidadesIniciales()
        {
            if (string.IsNullOrEmpty(formModel.ProvinciaId))
                return;

            cargandoLocalidades = true;
            try
            {
                var token = BackendToken;
                if (!string.IsNullOrEmpty(token))
                {
                    localidades = await _territorioService.ObtenerLocalidadesPorProvinciaAsync(formModel.ProvinciaId);

                    // Buscar la localidad por nombre si tenemos el nombre guardado
                    if (localidades != null && !string.IsNullOrEmpty(nombreLocalidadPaciente))
                    {
                        var localidadEncontrada = localidades.FirstOrDefault(l =>
                            l.nombre?.Equals(nombreLocalidadPaciente, StringComparison.OrdinalIgnoreCase) == true);
                        if (localidadEncontrada != null)
                        {
                            formModel.LocalidadId = localidadEncontrada._id ?? string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mensajeError = $"Error al cargar las localidades: {ex.Message}";
            }
            finally
            {
                cargandoLocalidades = false;
            }
        }

        private void ActivarModoEdicion()
        {
            modoEdicion = true;
            mensajeExito = string.Empty;
            mensajeError = string.Empty;

            // Si hay provincia seleccionada pero no localidades cargadas, cargarlas
            if (!string.IsNullOrEmpty(formModel.ProvinciaId) && (localidades == null || localidades.Count == 0))
            {
                _ = CargarLocalidadesIniciales();
            }
        }

        private async Task CancelarEdicion()
        {
            modoEdicion = false;
            mensajeExito = string.Empty;
            mensajeError = string.Empty;
            InicializarFormulario(); // Restaurar valores originales

            // Recargar localidades de la provincia original si hay una seleccionada
            await CargarLocalidadesIniciales();
        }

        // Propiedad para el celular con setter personalizado
        private string CelularValue
        {
            get => celular;
            set
            {
                string valor = value ?? "";

                // Eliminar cualquier carácter que no sea número
                valor = SoloNumerosRegex.Replace(valor, "");

                // Si empieza con 0 o 15, dejarlo vacío
                if (IniciaConCeroRegex.IsMatch(valor))
                {
                    celular = IniciaConCeroRegex.Replace(valor, "");
                    errorCelular = false;
                    return;
                }
                if (IniciaConQuinceRegex.IsMatch(valor))
                {
                    celular = IniciaConQuinceRegex.Replace(valor, "");
                    errorCelular = false;
                    return;
                }

                // Limitar a máximo 10 dígitos
                if (valor.Length > LONGITUD_MAXIMA_CELULAR)
                {
                    valor = valor.Substring(0, LONGITUD_MAXIMA_CELULAR);
                }

                celular = valor;
                errorCelular = false;
            }
        }

        // Propiedad para el código postal con setter personalizado
        private string CodigoPostalValue
        {
            get => formModel.CodigoPostal;
            set
            {
                string valor = value ?? "";

                // Eliminar cualquier carácter que no sea número
                valor = SoloNumerosRegex.Replace(valor, "");

                // Limitar a máximo 4 dígitos
                if (valor.Length > 4)
                {
                    valor = valor.Substring(0, 4);
                }

                formModel.CodigoPostal = valor;
            }
        }

        private async Task GuardarDatos()
        {
            if (paciente == null) return;

            mensajeExito = string.Empty;
            mensajeError = string.Empty;

            var token = BackendToken;
            if (string.IsNullOrEmpty(token))
            {
                mensajeError = "No se pudo obtener el token de autenticación.";
                return;
            }

            _spinnerService.Show();
            try
            {
                var pacienteActualizado = paciente.MapToActualizarPacienteDto();

                // Actualizar datos personales
                pacienteActualizado.Alias = string.IsNullOrWhiteSpace(formModel.Alias) ? null : formModel.Alias.Trim();
                pacienteActualizado.Genero = formModel.Genero;

                // Actualizar domicilio - buscar por nombre en lugar de ID
                var provinciaSeleccionada = provincias?.FirstOrDefault(p => p.id == formModel.ProvinciaId);
                if (provinciaSeleccionada == null)
                {
                    mensajeError = "Debe seleccionar una provincia válida.";
                    return;
                }

                var localidadSeleccionada = localidades?.FirstOrDefault(l => l._id == formModel.LocalidadId);
                if (localidadSeleccionada == null)
                {
                    mensajeError = "Debe seleccionar una localidad válida.";
                    return;
                }

                // Obtener o crear dirección principal
                var direccionPrincipal = _pacienteService.ObtenerDireccionPrioritaria(pacienteActualizado);
                if (direccionPrincipal == null)
                {
                    direccionPrincipal = new ActualizarPacienteDireccionDto
                    {
                        Activo = true,
                        Ranking = 1,
                        GeoReferencia = new List<double>()
                    };
                    pacienteActualizado.Direccion.Add(direccionPrincipal);
                }

                direccionPrincipal.Valor = formModel.Direccion?.Trim();
                direccionPrincipal.CodigoPostal = formModel.CodigoPostal?.Trim();

                if (direccionPrincipal.Ubicacion == null)
                {
                    direccionPrincipal.Ubicacion = new ActualizarPacienteUbicacionDto();
                }

                direccionPrincipal.Ubicacion.Provincia = new ActualizarPacienteReferenciaDto
                {
                    IdInterno = provinciaSeleccionada._id,
                    Id = provinciaSeleccionada.id,
                    Nombre = provinciaSeleccionada.nombre
                };

                direccionPrincipal.Ubicacion.Localidad = new ActualizarPacienteReferenciaDto
                {
                    IdInterno = localidadSeleccionada._id,
                    Nombre = localidadSeleccionada.nombre,
                    Id = localidadSeleccionada.id ?? localidadSeleccionada._id
                };

                // Obtener georeferencia de la dirección actualizada
                string direccionCompleta = $"{direccionPrincipal.Valor}, {direccionPrincipal.Ubicacion.Localidad?.Nombre}";
                userLocation georeferencia = await _pacienteService.ObtenerGeoreferenciaPaciente(direccionCompleta);
                if (georeferencia != null)
                {
                    direccionPrincipal.GeoReferencia = new List<double> { georeferencia.lat, georeferencia.lng };
                }

                // Actualizar contactos
                // Actualizar o crear email
                var emailContacto = pacienteActualizado.Contacto.FirstOrDefault(c => c.Tipo == "email");
                if (!string.IsNullOrWhiteSpace(formModel.Email))
                {
                    if (emailContacto == null)
                    {
                        emailContacto = new ActualizarPacienteContactoDto
                        {
                            Tipo = "email",
                            Activo = true,
                            Ranking = 0
                        };
                        pacienteActualizado.Contacto.Add(emailContacto);
                    }
                    emailContacto.Valor = formModel.Email.Trim();
                }
                else if (emailContacto != null)
                {
                    emailContacto.Valor = string.Empty;
                }

                // Validar celular antes de guardar
                if (string.IsNullOrWhiteSpace(celular))
                {
                    mensajeError = "El celular es obligatorio.";
                    return;
                }

                if (celular.Length != LONGITUD_MAXIMA_CELULAR)
                {
                    mensajeError = $"El celular debe tener exactamente {LONGITUD_MAXIMA_CELULAR} dígitos.";
                    return;
                }

                // Actualizar o crear celular
                var celularContacto = pacienteActualizado.Contacto.FirstOrDefault(c => c.Tipo == "celular");
                if (celularContacto == null)
                {
                    celularContacto = new ActualizarPacienteContactoDto
                    {
                        Tipo = "celular",
                        Activo = true,
                        Ranking = 1
                    };
                    pacienteActualizado.Contacto.Add(celularContacto);
                }
                celularContacto.Valor = celular;

                // Guardar cambios
                var pacienteGuardado = await _pacienteService.ModificarDatos(paciente.id, pacienteActualizado);

                if (pacienteGuardado != null)
                {
                    paciente = pacienteGuardado;
                    mensajeExito = "Los datos se han actualizado correctamente.";
                    modoEdicion = false;
                }
                else
                {
                    mensajeError = "No se pudo guardar los cambios. Por favor, intente nuevamente.";
                }
            }
            catch (Exception)
            {
                mensajeError = $"Error al guardar los datos";
            }
            finally
            {
                _spinnerService.Hide();
            }
        }
    }
}
