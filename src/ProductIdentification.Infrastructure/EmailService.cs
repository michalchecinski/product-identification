using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProductIdentification.Infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly string _fromEmail;
        private readonly string _emailPassword;
        private readonly string _smtpHost;
        private int _smtpPort;

        public EmailService(AppSettings settings)
        {
            _fromEmail = settings.EmailFrom;
            _emailPassword = settings.EmailPassword;
            _smtpHost = settings.EmailSmtpHost;
            _smtpPort = settings.EmailSmtpPort;
        }

        public async Task SendEmailAsync(string email, string title, string htmlMessage)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_fromEmail);
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
            smtp.Port = _smtpPort;
            smtp.Host = _smtpHost;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_fromEmail, _emailPassword);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            return smtp;
        }
    }
}