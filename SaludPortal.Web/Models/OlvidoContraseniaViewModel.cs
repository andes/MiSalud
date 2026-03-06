using System.ComponentModel.DataAnnotations;

namespace SaludPortal.Web.Models;

public class OlvidoContraseniaViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El correo electrónico es requerido.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    public string Email { get; set; } = string.Empty;
}
