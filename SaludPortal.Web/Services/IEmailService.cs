using System.Net.Mail;
using System.Threading.Tasks;

namespace SaludPortal.Web.Services
{
    public interface IEmailService
    {
        Task SendAsync(string subject, string body, Attachment? attachment = null);
    }
}