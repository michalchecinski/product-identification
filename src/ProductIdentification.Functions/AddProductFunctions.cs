using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Models.Messages;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Functions
{
    public class AddProductFunctions
    {
        private readonly IProductTrainingRepository _productTrainingRepository;

        public AddProductFunctions(IProductTrainingRepository productTrainingRepository)
        {
            _productTrainingRepository = productTrainingRepository;
        }

        [FunctionName(nameof(AddProductToIdentifyService))]
        public void AddProductToIdentifyService([QueueTrigger(QueueNames.AddProduct, Connection = "Storage")]
                                                AddProductMessage message, ILogger log)
        {
            log.LogInformation("AddProductToIdentifyService function called");
            log.LogInformation($"Message: {message}");
            
            var productTrainingModel = new ProductTrainingModel(message.ProductId);
            _productTrainingRepository.Add(productTrainingModel);
        }
    }
}