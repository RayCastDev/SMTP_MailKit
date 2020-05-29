using System.Threading.Tasks;
using NotificationService.Model;

namespace NotificationService.Service
{
    public interface ISmtpProvider
    {
        Task<bool> SendEmailAsync(EmailMessage emailMessage);
    }
}
