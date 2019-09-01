using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
                                                string message, ILogger log)
        {
            var parsedMessage = JsonConvert.DeserializeObject<AddProductMessage>(message);
            var productTrainingModel = new ProductTrainingModel(parsedMessage.ProductId);
            _productTrainingRepository.Add(productTrainingModel);
        }
    }
}