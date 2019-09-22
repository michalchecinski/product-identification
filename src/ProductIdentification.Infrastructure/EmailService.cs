using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProductIdentification.Infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly string _fromEmail;
        private readonly string _emailPassword;
        
        public EmailService(AppSettings settings)
        {
            _fromEmail = settings.EmailFrom;
            _emailPassword = settings.EmailPassword;
        }
        
        public async Task SendEmail(string email, string title, string htmlMessage)
        {
            MailMessage message = new MailMessage(); 
            message.From = new MailAddress("FromMailAddress");  
            message.To.Add(new MailAddress(email));  
            message.Subject = title;  
            message.IsBodyHtml = true;
            message.Body = htmlMessage;
            
            var smtp = GetSmtpClient();
            smtp.Send(message); 
        }

        private static SmtpClient GetSmtpClient()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("FromMailAddress", "password");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            return smtp;
        }
    }
}