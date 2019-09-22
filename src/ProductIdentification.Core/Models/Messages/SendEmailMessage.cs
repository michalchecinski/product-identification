namespace ProductIdentification.Core.Models.Messages
{
    public class SendEmailMessage : IQueueMessage
    {
        public SendEmailMessage(string email, string title, string htmlMessage)
        {
            Email = email;
            Title = title;
            HtmlMessage = htmlMessage;
        }
        
        public string Email { get; set; }
        public string Title { get; set; }
        public string HtmlMessage { get; set; }
    }
}