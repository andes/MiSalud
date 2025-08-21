using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using SaludPortal.Web.Models;
using SaludPortal.Web.Services;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Routing;

namespace SaludPortal.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OlvidoContraseniaController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<OlvidoContraseniaController> _logger;
        private readonly IConfiguration _config;

        public OlvidoContraseniaController(IEmailService emailService, ILogger<OlvidoContraseniaController> logger, IConfiguration config) // Modificado
        {
            _emailService = emailService;
            _logger = logger;
            _config = config;
        }

        [HttpPost("solicitud")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Solicitud([FromForm] OlvidoContraseniaRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            byte[]? fileBytes = null;
            string? fileName = null;
            string? mediaType = null;

            if (request.FotoFrente != null && request.FotoFrente.Length > 0)
            {
                if (request.FotoFrente.Length > 5 * 1024 * 1024)
                    return BadRequest("La imagen supera los 5MB.");
                using var ms = new MemoryStream();
                await request.FotoFrente.CopyToAsync(ms, ct);
                fileBytes = ms.ToArray();
                fileName = request.FotoFrente.FileName;
                mediaType = request.FotoFrente.ContentType;
            }

            var body = $@"Nueva solicitud de datos de acceso desde Web Mi Salud:
                        Nombre: {request.Nombre}
                        Apellido: {request.Apellido}
                        Documento: {request.Documento}
                        Email: {request.Email}
                        Teléfono: {request.Telefono}";

            var subject = "Solicitud de recupero de datos de acceso de Andes - Mi Salud";

            try
            {
                if (bool.TryParse(_config["Smtp:BackgroundSend"], out var bg) && bg)
                {
                    _logger.LogInformation("Encolando envío de email (bg=true)");
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            System.Net.Mail.Attachment? att = null;
                            if (fileBytes != null)
                            {
                                var msFile = new MemoryStream(fileBytes);
                                att = new System.Net.Mail.Attachment(msFile, fileName ?? "adjunto.bin");
                                if (!string.IsNullOrEmpty(mediaType))
                                    att.ContentType.MediaType = mediaType;
                            }

                            await _emailService.SendAsync(subject, body, att);

                            att?.Dispose();
                        }
                        catch (Exception exBg)
                        {
                            _logger.LogError(exBg, "Error en envío en background de OlvidoContrasenia");
                        }
                    });
                    return Ok(new { message = "Solicitud encolada." });
                }

                System.Net.Mail.Attachment? inlineAtt = null;
                if (fileBytes != null)
                {
                    var msFile = new MemoryStream(fileBytes);
                    inlineAtt = new System.Net.Mail.Attachment(msFile, fileName ?? "adjunto.bin");
                    if (!string.IsNullOrEmpty(mediaType))
                        inlineAtt.ContentType.MediaType = mediaType;
                }

                await _emailService.SendAsync(subject, body, inlineAtt);
                inlineAtt?.Dispose();

                return Ok(new { message = "Solicitud enviada." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando solicitud OlvidoContrasenia");
                return StatusCode(500, "Error enviando la solicitud.");
            }
        }
    }
}