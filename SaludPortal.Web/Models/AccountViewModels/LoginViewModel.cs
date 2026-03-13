namespace SaludPortal.Web.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;
    public record LoginViewModel
    {
        [Required(ErrorMessage = "El campo Email es requerido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es requerido")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
