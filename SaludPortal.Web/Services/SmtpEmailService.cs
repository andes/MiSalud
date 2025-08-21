using System;
using System.Net.Mail; // Solo para el tipo Attachment que ya usas externamente
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace SaludPortal.Web.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(IConfiguration config, ILogger<SmtpEmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendAsync(string subject, string body, Attachment? attachment = null)
        {
            var section = _config.GetSection("Smtp");
            var host = section["Host"] ?? throw new InvalidOperationException("Smtp:Host no configurado");
            var port = int.Parse(section["Port"] ?? "25");
            var user = section["User"];
            var pass = section["Password"];
            var from = section["From"] ?? user ?? "noreply@local";
            var to = section["To"] ?? from;
            var defaultSubject = section["Subject"] ?? "Solicitud";
            var timeoutMs = int.Parse(section["TimeoutMs"] ?? "30000");
            var securitySetting = (section["Security"] ?? "").Trim();
            var enableSslLegacy = bool.Parse(section["EnableSsl"] ?? "false");
            var skipCertValidation = bool.Parse(section["SkipCertValidation"] ?? "false");

            // Determinar protocolo de seguridad
            SecureSocketOptions socketOptions = SecureSocketOptions.Auto;
            if (!string.IsNullOrEmpty(securitySetting))
            {
                socketOptions = securitySetting switch
                {
                    "Ssl" => SecureSocketOptions.SslOnConnect,
                    "StartTls" => SecureSocketOptions.StartTls,
                    "StartTlsWhenAvailable" => SecureSocketOptions.StartTlsWhenAvailable,
                    "None" => SecureSocketOptions.None,
                    _ => SecureSocketOptions.Auto
                };
            }
            else
            {
                // Compatibilidad con tu config anterior
                if (enableSslLegacy)
                {
                    // Puerto 465 suele requerir SslOnConnect; 587 → StartTls
                    socketOptions = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
                }
                else
                {
                    socketOptions = SecureSocketOptions.StartTlsWhenAvailable;
                }
            }

            var mime = new MimeMessage();
            mime.From.Add(MailboxAddress.Parse(from));
            foreach (var addr in to.Split(';', ',', StringSplitOptions.RemoveEmptyEntries))
                mime.To.Add(MailboxAddress.Parse(addr.Trim()));
            mime.Subject = subject ?? defaultSubject;

            var builder = new BodyBuilder
            {
                TextBody = body,
                HtmlBody = null // Si en el futuro quieres soportar HTML agrega builder.HtmlBody
            };

            if (attachment != null)
            {
                try
                {
                    attachment.ContentStream.Position = 0;
                    builder.Attachments.Add(attachment.Name, attachment.ContentStream, ContentType.Parse(attachment.ContentType?.MediaType ?? "application/octet-stream"));
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "No se pudo adjuntar el archivo, se enviará sin adjunto");
                }
            }

            mime.Body = builder.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient
            {
                Timeout = timeoutMs
            };

            if (skipCertValidation)
            {
                client.ServerCertificateValidationCallback = (s, cert, chain, errors) => true;
            }

            var cts = new CancellationTokenSource(timeoutMs + 5000);

            _logger.LogInformation("Conectando SMTP Host={Host} Port={Port} Security={Security} TimeoutMs={Timeout}", host, port, socketOptions, timeoutMs);

            try
            {
                await client.ConnectAsync(host, port, socketOptions, cts.Token);

                if (!string.IsNullOrEmpty(user))
                {
                    // Algunos servidores requieren autenticación explícita
                    await client.AuthenticateAsync(user, pass, cts.Token);
                }

                await client.SendAsync(mime, cts.Token);
                await client.DisconnectAsync(true, cts.Token);

                _logger.LogInformation("Email enviado a {Destinatarios}", string.Join(',', mime.To));
            }
            catch (OperationCanceledException oce)
            {
                _logger.LogError(oce, "Timeout enviando email (>{Timeout} ms)", timeoutMs);
                throw;
            }
            catch (SmtpCommandException sce)
            {
                _logger.LogError(sce, "SMTP command error StatusCode={StatusCode} Response={Response}", sce.StatusCode, sce.Message);
                throw;
            }
            catch (SmtpProtocolException spe)
            {
                _logger.LogError(spe, "SMTP protocol error");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general enviando email");
                throw;
            }
        }
    }
}