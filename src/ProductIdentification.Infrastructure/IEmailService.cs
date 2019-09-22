using System.Threading.Tasks;

namespace ProductIdentification.Infrastructure
{
    public interface IEmailService
    {
        Task SendEmail(string email, string title, string htmlMessage);
    }
}