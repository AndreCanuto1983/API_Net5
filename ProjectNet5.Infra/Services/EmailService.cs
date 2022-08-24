using Core.Models.BaseModel;
using Domain.Interfaces.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infra.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IConfiguration configuration, 
            ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject)
        {
            try
            {
                var emailSettingsSection = _configuration.GetSection("EmailConfigs");
                var emailSettings = emailSettingsSection.Get<EmailSettings>();

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailSettings.From, "Simis");
                mailMessage.To.Add(toEmail);
                mailMessage.CC.Add(emailSettings.From);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = HtmlContentGenerate("André");

                SmtpClient smtpClient = new SmtpClient(emailSettings.Smtp, Convert.ToInt32(emailSettings.Port));
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.From, emailSettings.Key);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EmailService][SendEmailAsync] => EXCEPTION: {ex.Message}", ex.Message);
                throw;
            }
        }

        private static string HtmlContentGenerate(string name)
        {
            //busca e lê o arquivo
            string pathToHTMLFile = @"~\WebApi\Models\EmailContent.html";

            //faz a leitura e adiciona na variável
            string body = File.ReadAllText(pathToHTMLFile);

            //altera as variáveis no html
            body = body.Replace("{name}", name);
            body = body.Replace("{tel}", "(16)994-2516");
            body = body.Replace("{btn}", "Resetar Senha");
            body = body.Replace("{href}", "www.youtube.com");

            return body;
        }
    }
}
