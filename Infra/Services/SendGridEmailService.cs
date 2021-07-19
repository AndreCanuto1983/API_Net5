using Core.Models.BaseModel;
using Infra.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infra.Services
{
    public class SendGridEmailService : IEmailService
    {
        private IConfiguration _configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string emailTitle)
        {
            try
            {
                var emailSettingsSection = _configuration.GetSection("EmailConfigs");
                var emailSettings = emailSettingsSection.Get<EmailSettings>();

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailSettings.From, "Simis");
                mailMessage.To.Add(toEmail);
                mailMessage.CC.Add("simiscorporation@gmail.com");
                mailMessage.Subject = emailTitle;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = HtmlContentGenerate("André");

                SmtpClient smtpClient = new SmtpClient(emailSettings.Smtp, Convert.ToInt32(emailSettings.Port));
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.From, emailSettings.Key);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string HtmlContentGenerate(string name)
        {
            //busca e lê o arquivo
            //string pathToHTMLFile = @"C:\Users\andre\Documents\sims-app\back\WebApi\Models\EmailContent.html";
            string pathToHTMLFile = @"~\WebApi\Models\EmailContent.html";

            //faz a leitura e adiciona na variável
            string body = File.ReadAllText(pathToHTMLFile);

            //altera as variáveis no html
            body = body.Replace("{name}", name);
            body = body.Replace("{tel}", "(16)99615-3655");
            body = body.Replace("{btn}", "Resetar Senha");
            body = body.Replace("{href}", "www.youtube.com");

            #region Outra maneira de fazer

            //using (FileStream fs = File.Open(pathToHTMLFile, FileMode.Open, FileAccess.ReadWrite))
            //{
            //    using (StreamReader sr = new StreamReader(fs))
            //    {
            //        body = sr.ReadToEnd();
            //    }
            //}

            #endregion

            return body;
        }
    }
}
