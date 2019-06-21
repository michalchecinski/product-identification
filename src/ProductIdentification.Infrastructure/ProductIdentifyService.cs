using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private const string EndpointUrl = "https://westeurope.api.cognitive.microsoft.com";
        private const string PublishedModelName = "ProductIdentification";

        public ProductIdentifyService(IProductRepository productRepository, IOptions<AppSettings> settings)
        {
            _productRepository = productRepository;
            _predictionKey = settings.Value.CustomVisionPredictionKey;
            _trainingKey = settings.Value.CustomVisionTrainingKey;
            _projectId = settings.Value.CustomVisionProjectId;
        }

        public async Task<Product> IdentifyProduct(Stream image)
        {
            CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = _predictionKey,
                Endpoint = EndpointUrl
            };

            var result = endpoint.ClassifyImage(new Guid(_projectId), PublishedModelName, image);

            var tag = result.Predictions.OrderByDescending(x => x.Probability).FirstOrDefault()?.TagId;

            if (tag == null)
            {
                throw new Exception("Product not found");
            }

            return await _productRepository.Get(tag.Value);
        }

        public async Task<Product> AddProduct(IEnumerable<Stream> images, Product product)
        {
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = _trainingKey,
                Endpoint = EndpointUrl
            };

            var project = await trainingApi.GetProjectAsync(new Guid(_projectId));
            var tag = await trainingApi.CreateTagAsync(project.Id, product.TagName);

            foreach (var image in images)
            {
                await trainingApi.CreateImagesFromDataAsync(project.Id, image, new List<Guid>() {tag.Id});
            }

            product.CustomVisionTagId = tag.Id;

            return product;
        }
    }
}