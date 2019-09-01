using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Models.Messages;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Functions
{
    public class TrainModelFunctions
    {
        private const string TrainModelSchedule = "0 0 */2 * * *";
        private readonly IProductIdentifyService _productIdentifyService;
        private readonly IQueueService _queueService;
        private readonly IProductTrainingRepository _productTrainingRepository;

        public TrainModelFunctions(IProductIdentifyService productIdentifyService,
                          IQueueService queueService,
                          IProductTrainingRepository productTrainingRepository)
        {
            _productIdentifyService = productIdentifyService;
            _queueService = queueService;
            _productTrainingRepository = productTrainingRepository;
        }
        
        
        [FunctionName(nameof(TrainModelManually))]
        public async Task TrainModelManually([QueueTrigger(QueueNames.TrainModel, Connection = "Storage")] TrainModelMessage message,
                                       ILogger log)
        {
            log.LogInformation("PublishModel function called");
            log.LogInformation($"Message: {message}");

            await TrainModel();
        }


        [FunctionName(nameof(TrainModelScheduled))]
        public async Task TrainModelScheduled([TimerTrigger(TrainModelSchedule)] TimerInfo timeTrigger,
                                        ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
            
            await TrainModel();
        }

        private async Task TrainModel()
        {
            var toTrain = _productTrainingRepository.GetAllToTrain();
            
            if (toTrain.Any())
            {
                var projectId = await _productIdentifyService.TrainProjectAsync();
                var publishMessage = new PublishModelMessage(projectId);
                UpdateTrainingState(toTrain);
                await _queueService.SendDelayedMessageAsync(QueueNames.PublishModel, publishMessage,
                    TimeSpan.FromMinutes(10));
            }
        }

        private void UpdateTrainingState(IEnumerable<ProductTrainingModel> toTrain)
        {
            foreach (var productTrainingModel in toTrain)
            {
                productTrainingModel.TrainingState = TrainingStates.Training;
                _productTrainingRepository.Update(productTrainingModel);
            }
        }
    }
}