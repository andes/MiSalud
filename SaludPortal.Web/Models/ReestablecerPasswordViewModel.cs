using System.ComponentModel.DataAnnotations;

namespace SaludPortal.Web.Models;

public class ReestablecerPasswordViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El correo electrónico es requerido.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El código de activación es requerido.")]
    public string Codigo { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "La confirmación de contraseña es requerida.")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
