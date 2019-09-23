using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using ProductIdentification.Core.Models.Messages;

namespace ProductIdentification.Infrastructure
{
    public class QueueEmailSender : IEmailSender
    {
        private readonly IQueueService _queueService;

        public QueueEmailSender(IQueueService queueService)
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