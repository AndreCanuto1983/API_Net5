using System.Threading.Tasks;

namespace Domain.Interfaces.Service
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject);
    }
}
