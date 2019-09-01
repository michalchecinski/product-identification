namespace ProductIdentification.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using ProductIdentification.Core.Models;
    using ProductIdentification.Core.Models.Messages;
    using ProductIdentification.Core.Repositories;
    using ProductIdentification.Infrastructure;
    
    public class PublishModelFunctions
    {
        private readonly IProductIdentifyService _productIdentifyService;
        private readonly IQueueService _queueService;
        private readonly IProductTrainingRepository _productTrainingRepository;

        public PublishModelFunctions(IProductIdentifyService productIdentifyService,
                                   IQueueService queueService,
                                   IProductTrainingRepository productTrainingRepository)
        {
            _productIdentifyService = productIdentifyService;
            _queueService = queueService;
            _productTrainingRepository = productTrainingRepository;
        }
        
        [FunctionName(nameof(PublishModel))]
        public async Task PublishModel([QueueTrigger(QueueNames.PublishModel, Connection = "Storage")]PublishModelMessage message,
                                       ILogger log)
        {
            log.LogInformation("PublishModel function called");
            log.LogInformation($"Message with iterationId: {message.IterationId}");
            
            await PublishModel(message);
        }

        private async Task PublishModel(PublishModelMessage message)
        {
            var published = await _productIdentifyService.TryPublishIteration(message.IterationId);

            if (!published)
            {
                await _queueService.SendDelayedMessageAsync(QueueNames.PublishModel, message, TimeSpan.FromMinutes(5));
            }
            else
            {
                var trained = _productTrainingRepository.GetAllTraining();
                UpdateTrainedState(trained);
            }
        }

        private void UpdateTrainedState(IEnumerable<ProductTrainingModel> trained)
        {
            foreach (var productTrainingModel in trained)
            {
                productTrainingModel.TrainingState = TrainingStates.Training;
                _productTrainingRepository.Delete(productTrainingModel);
            }
        }
    }
}