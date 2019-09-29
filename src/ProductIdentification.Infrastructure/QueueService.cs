using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Newtonsoft.Json;
using ProductIdentification.Core.Models.Messages;

namespace ProductIdentification.Infrastructure
{
    public class QueueService : IQueueService
    {
        private readonly CloudQueueClient _queueClient;

        public QueueService(ISecretsFetcher secretsFetcher)
        {
            var storageAccount = CloudStorageAccount.Parse(secretsFetcher.GetStorageConnectionString);
            _queueClient = storageAccount.CreateCloudQueueClient();
        }
        
        public async Task SendMessageAsync(string queueName, IQueueMessage message)
        {
            var queue = _queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            var jsonMessage = JsonConvert.SerializeObject(message);
            
            var cloudMessage = new CloudQueueMessage(jsonMessage);
            await queue.AddMessageAsync(cloudMessage);
        }

        public async Task SendDelayedMessageAsync(string queueName, IQueueMessage message, TimeSpan timeSpan)
        {
            var queue = _queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            var jsonMessage = JsonConvert.SerializeObject(message);
            
            var cloudMessage = new CloudQueueMessage(jsonMessage);
            await queue.AddMessageAsync(cloudMessage, null, timeSpan, null, null);
        }
    }
}