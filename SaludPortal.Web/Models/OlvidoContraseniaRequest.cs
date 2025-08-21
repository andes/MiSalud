using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SaludPortal.Web.Models
{
    public class OlvidoContraseniaRequest
    {
        [Required(ErrorMessage ="El campo Nombre es requerido"), StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo Apellido es requerido"), StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo Documento es requerido"), StringLength(20)]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo Email es requerido"), EmailAddress, StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo Teléfono celular es requerido"), Phone, StringLength(30)]
        public string Telefono { get; set; } = string.Empty;

        public IFormFile? FotoFrente { get; set; }
    }
}