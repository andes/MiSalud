using System.ComponentModel.DataAnnotations;

namespace SaludPortal.Web.Models;

public class RegistroViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El correo electrónico es requerido.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    [StringLength(100, ErrorMessage = "El correo electrónico no puede superar los 100 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El documento es requerido.")]
    [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El documento debe tener 7 u 8 dígitos.")]
    public string Documento { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El sexo es requerido.")]
    [RegularExpression("^(masculino|femenino)$", ErrorMessage = "El sexo debe ser masculino o femenino.")]
    public string Sexo { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El número de teléfono es requerido.")]
    [RegularExpression(@"^(?!0)(?!15)[1-9]\d{9}$", ErrorMessage = "El número de teléfono debe tener 10 dígitos, no puede empezar con 0 ni con 15.")]
    public string Telefono { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El número de trámite es requerido.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "El número de trámite debe tener 11 dígitos.")]
    public string NroTramite { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido es requerido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres.")]
    public string Apellidos { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Nombres { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El ejemplar es requerido.")]
    [RegularExpression(@"^[A-Z]$", ErrorMessage = "El ejemplar debe ser una letra mayúscula (A, B, C, etc.).")]
    public string Ejemplar { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es requerida.")]
    public DateTime FechaNacimiento { get; set; }
}
