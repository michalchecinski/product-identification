using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ProductIdentification.Core.Models.Messages;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Functions
{
    public class EmailSenderFunction
    {
        private readonly IEmailService _emailService;

        public EmailSenderFunction(IEmailService emailService)
        {
            _emailService = emailService;
        }
        
        [FunctionName(nameof(EmailSender))]
        public void EmailSender([QueueTrigger(QueueNames.SendEmail, Connection = "Storage")]
                               SendEmailMessage emailMessage, ILogger log)
        {
            log.LogInformation($"Sending email to: {emailMessage.Email} {emailMessage.Title}");

            _emailService.SendEmailAsync(emailMessage.Email, emailMessage.Title, emailMessage.HtmlMessage);
            
            log.LogInformation($"Email sent to: {emailMessage.Email} {emailMessage.Title}");
        }
    }
}