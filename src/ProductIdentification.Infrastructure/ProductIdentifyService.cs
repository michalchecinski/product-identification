using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.Extensions.Options;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Repositories;

namespace ProductIdentification.Infrastructure
{
    public class ProductIdentifyService : IProductIdentifyService
    {
        private readonly IProductRepository _productRepository;
        private readonly string _predictionKey;
        private readonly string _projectId;
        private readonly string _trainingKey;
        private readonly string _predictionId;
        private readonly string _endpointUrl;
        private const string PublishedModelName = "ProductIdentification";

        public ProductIdentifyService(IProductRepository productRepository, IOptions<AppSettings> settings)
        {
            _productRepository = productRepository;
            var settingsValue = settings.Value;
            _predictionKey = settingsValue.CustomVisionPredictionKey;
            _trainingKey = settingsValue.CustomVisionTrainingKey;
            _projectId = settingsValue.CustomVisionProjectId;
            _predictionId = settingsValue.CustomVisionPredictionId;
            _endpointUrl = settingsValue.CustomVisionEndpoint;
        }

        public async Task<Product> IdentifyProduct(Stream image)
        {
            CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = _predictionKey,
                Endpoint = _endpointUrl
            };

            image.Seek(0, SeekOrigin.Begin);
            var result = await endpoint.ClassifyImageAsync(new Guid(_projectId), PublishedModelName, image);

            var tag = result.Predictions.OrderByDescending(x => x.Probability).FirstOrDefault()?.TagId;

            if (tag == null)
            {
                throw new Exception("Product not found");
            }

            return await _productRepository.Get(tag.Value);
        }

        public async Task<Product> AddProduct(List<IFormFile> images, Product product)
        {
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = _trainingKey,
                Endpoint = _endpointUrl
            };

            var project = await trainingApi.GetProjectAsync(new Guid(_projectId));
            var tag = await trainingApi.CreateTagAsync(project.Id, product.TagName);

            foreach (var image in images)
            {
                using (var stream = new MemoryStream())
                {
                    await image.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    await trainingApi.CreateImagesFromDataAsync(project.Id, stream, new List<Guid>() { tag.Id });
                }
            }

#pragma warning disable 4014
            Task.Run(async () =>
            {
                var iteration = await trainingApi.TrainProjectAsync(project.Id);

                while (iteration.Status == "Training")
                {
                    Thread.Sleep(1000);

                    iteration = await trainingApi.GetIterationAsync(project.Id, iteration.Id);
                }

                await trainingApi.PublishIterationAsync(project.Id, iteration.Id, PublishedModelName, _predictionId);

            }).ConfigureAwait(false);
#pragma warning restore 4014

            product.CustomVisionTagId = tag.Id;

            return product;
        }
    }
}