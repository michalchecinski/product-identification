using System.Threading.Tasks;
using ProductIdentification.Core.Models.Messages;

namespace ProductIdentification.Infrastructure
{
    public interface IQueueService
    {
        Task SendMessageAsync(string queueName, IQueueMessage message);
    }
}