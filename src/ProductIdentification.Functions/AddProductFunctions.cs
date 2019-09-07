﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Models.Messages;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Infrastructure;
using ProductIdentification.Common;

namespace ProductIdentification.Functions
{
    public class AddProductFunctions
    {
        private readonly IProductTrainingRepository _productTrainingRepository;
        private readonly IProductIdentifyService _productIdentifyService;
        private readonly IProductRepository _productRepository;
        private readonly IFileRepository _fileRepository;

        public AddProductFunctions(IProductTrainingRepository productTrainingRepository,
                                   IProductIdentifyService productIdentifyService,
                                   IProductRepository productRepository,
                                   IFileRepository fileRepository)
        {
            _productTrainingRepository = productTrainingRepository;
            _productIdentifyService = productIdentifyService;
            _productRepository = productRepository;
            _fileRepository = fileRepository;
        }

        [FunctionName(nameof(AddProductToIdentifyService))]
        public async Task AddProductToIdentifyService([QueueTrigger(QueueNames.AddProduct, Connection = "Storage")]
                                                AddProductMessage message, ILogger log)
        {
            log.LogInformation("AddProductToIdentifyService function called");
            log.LogInformation($"Message: {message}");

            var product = await _productRepository.Get(message.ProductId);

            var folder = product.StoragePathOriginal();

            var fileNames = await _fileRepository.FileNamesList(folder);

            var images = new List<Stream>();

            foreach (var fileName in fileNames)
            {
                var file = await _fileRepository.GetFileContentAsync(folder, fileName);
                images.Add(file);
            }
            
            var updatedProduct = await _productIdentifyService.AddProduct(images, product);

            await _productRepository.UpdateProductAsync(updatedProduct);
            
            var productTrainingModel = new ProductTrainingModel(message.ProductId);
            _productTrainingRepository.Add(productTrainingModel);
        }
    }
}