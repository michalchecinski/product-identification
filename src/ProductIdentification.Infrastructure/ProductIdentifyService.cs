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

        public ProductIdentifyService(IProductRepository productRepository, AppSettings settings)
        {
            _productRepository = productRepository;
            _predictionKey = settings.CustomVisionPredictionKey;
            _trainingKey = settings.CustomVisionTrainingKey;
            _projectId = settings.CustomVisionProjectId;
            _predictionId = settings.CustomVisionPredictionId;
            _endpointUrl = settings.CustomVisionEndpoint;
        }

        public async Task<Product> IdentifyProduct(Stream image)
        {
            CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = _predictionKey,
                Endpoint = _endpointUrl
            };

            image.Position = 0;
            var result = await endpoint.ClassifyImageAsync(new Guid(_projectId), PublishedModelName, image);

            var tag = result.Predictions
                            .Where(x => x.Probability > 0.2)
                            .OrderByDescending(x => x.Probability)
                            .FirstOrDefault()?.TagId;

            if (tag == null)
            {
                return null;
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
                    await trainingApi.CreateImagesFromDataAsync(project.Id, stream, new List<Guid>() {tag.Id});
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