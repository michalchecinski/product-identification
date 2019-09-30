using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProductIdentification.Infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly ISecretsFetcher _secretsFetcher;

        public EmailService(ISecretsFetcher secretsFetcher)
        {
            _secretsFetcher = secretsFetcher;
        }

        public async Task SendEmailAsync(string email, string title, string htmlMessage)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_secretsFetcher.GetEmailFrom);
            message.To.Add(new MailAddress(email));
            message.Subject = title;
            message.IsBodyHtml = true;
            message.Body = htmlMessage;

            var smtp = GetSmtpClient();
            smtp.Send(message);
        }

        private SmtpClient GetSmtpClient()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Port = _secretsFetcher.GetEmailSmtpPort;
            smtp.Host = _secretsFetcher.GetEmailSmtpHost;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_secretsFetcher.GetEmailUsername, _secretsFetcher.GetEmailPassword);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            return smtp;
        }
    }
}