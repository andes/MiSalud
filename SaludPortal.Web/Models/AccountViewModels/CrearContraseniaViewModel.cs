namespace SaludPortal.Web.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;

    public record CrearContraseniaViewModel
    {
        [Required(ErrorMessage = "El campo Email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string NuevaContrasenia { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [Compare(nameof(NuevaContrasenia), ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasenia { get; set; } = string.Empty;
    }
}
