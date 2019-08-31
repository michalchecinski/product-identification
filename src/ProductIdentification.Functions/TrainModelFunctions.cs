using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProductIdentification.Core.Models.Messages;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Functions
{
    public class TrainModelFunctions
    {
        private readonly IProductIdentifyService _productIdentifyService;
        private readonly IQueueService _queueService;

        public TrainModelFunctions(IProductIdentifyService productIdentifyService,
                          IQueueService queueService)
        {
            _productIdentifyService = productIdentifyService;
            _queueService = queueService;
        }
        
        
        [FunctionName(nameof(TrainModelManually))]
        public async Task TrainModelManually([QueueTrigger(QueueNames.TrainModel, Connection = "Storage")] TrainModelMessage message,
                                       ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {message}");

            await TrainModel();
        }


        [FunctionName(nameof(TrainModelScheduled))]
        public async Task TrainModelScheduled([TimerTrigger("0 0 */5 * * *")] TimerInfo timeTrigger,
                                        ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
            
            await TrainModel();
        }
        private async Task TrainModel()
        {
            var projectId = await _productIdentifyService.TrainProjectAsync();
            var publishMessage = new PublishModelMessage(projectId);
            await _queueService.SendMessageAsync(QueueNames.PublishModel, publishMessage);
        }

        [FunctionName(nameof(PublishModel))]
        public async Task PublishModel([QueueTrigger(QueueNames.PublishModel, Connection = "Storage")]PublishModelMessage message,
                                        ILogger log)
        {
            var published = await _productIdentifyService.TryPublishIteration(message.IterationId);

            if (!published)
            {
                await _queueService.SendMessageAsync(QueueNames.PublishModel, message);
            }
        }
    }
}