using System.Threading.Tasks;

namespace Infra.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject);
    }
}
