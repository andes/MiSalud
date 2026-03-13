namespace SaludPortal.Web.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;

    public record IngresarCodigoActivacionViewModel
    {
        [Required(ErrorMessage = "El campo Email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código de activación es requerido")]
        public string CodigoActivacion { get; set; } = string.Empty;
    }
}
