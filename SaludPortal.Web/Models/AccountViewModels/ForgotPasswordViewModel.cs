using System.ComponentModel.DataAnnotations;

namespace SaludPortal.Web.Models.AccountViewModels
{
    public record ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }
    }
}
