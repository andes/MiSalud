using System.ComponentModel.DataAnnotations;

namespace SaludPortal.Web.Models
{
    public class MisDatosFormModel
    {
        public string Alias { get; set; } = string.Empty;

        [Required(ErrorMessage = "El género es obligatorio")]
        public string Genero { get; set; } = string.Empty;

        [Required(ErrorMessage = "La provincia es obligatoria")]
        public string ProvinciaId { get; set; } = string.Empty;

        [Required(ErrorMessage = "La localidad es obligatoria")]
        public string LocalidadId { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código postal es obligatorio")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "El código postal debe tener exactamente 4 dígitos")]
        public string CodigoPostal { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        public string Email { get; set; } = string.Empty;

        public static ValidationResult? ValidarCelular(string celular, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(celular))
            {
                return new ValidationResult("El celular es obligatorio");
            }

            // Validar: solo números, sin cero inicial, sin 15, exactamente 10 dígitos
            if (!System.Text.RegularExpressions.Regex.IsMatch(celular, @"^\d{10}$"))
            {
                return new ValidationResult("El celular debe tener exactamente 10 dígitos numéricos");
            }

            // Verificar que no empiece con 0
            if (celular.StartsWith("0"))
            {
                return new ValidationResult("El celular no debe comenzar con cero");
            }

            // Verificar que no empiece con 15
            if (celular.StartsWith("15"))
            {
                return new ValidationResult("El celular no debe incluir el prefijo 15");
            }

            return ValidationResult.Success;
        }
    }
}
