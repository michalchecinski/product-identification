using System.Threading.Tasks;
using ProductIdentification.Core.Models.Messages;

namespace ProductIdentification.Infrastructure
{
    public class QueueService : IQueueService
    {
        public Task SendMessageAsync(string queueName, IQueueMessage message)
        {
            return Task.CompletedTask;
        }
    }
}