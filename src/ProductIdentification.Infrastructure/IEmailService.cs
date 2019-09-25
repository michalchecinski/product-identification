using System.Threading.Tasks;

namespace ProductIdentification.Infrastructure
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string title, string htmlMessage);
    }
}