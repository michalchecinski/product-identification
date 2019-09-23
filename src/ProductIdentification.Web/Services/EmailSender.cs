using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using ProductIdentification.Core.Models.Messages;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Web.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IQueueService _queueService;

        public EmailSender(IQueueService queueService)
        {
            _queueService = queueService;
        }
        
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new SendEmailMessage(email, subject, htmlMessage);
            await _queueService.SendMessageAsync(QueueNames.SendEmail, emailMessage);
        }
    }
}