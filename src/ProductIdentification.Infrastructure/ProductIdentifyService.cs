﻿using System;
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
using ProductIdentification.Core.DomainModels;
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

        public async Task<Product> AddProduct(List<Stream> images, Product product)
        {
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = _trainingKey,
                Endpoint = _endpointUrl
            };

            var project = await trainingApi.GetProjectAsync(new Guid(_projectId));
            Tag tag;
            
            try
            {
                tag = await trainingApi.CreateTagAsync(project.Id, product.TagName);
            }
            catch (CustomVisionErrorException e)
            {
                throw new Exception(e.Body.Message, e);
            }

            foreach (var image in images)
            {
                image.Seek(0, SeekOrigin.Begin);
                await trainingApi.CreateImagesFromDataAsync(project.Id, image, new List<Guid>() {tag.Id});
            }

            product.CustomVisionTagId = tag.Id;

            return product;
        }

        public async Task<Guid> TrainProjectAsync()
        {
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = _trainingKey,
                Endpoint = _endpointUrl
            };

            var project = await trainingApi.GetProjectAsync(new Guid(_projectId));

            var iteration = await trainingApi.TrainProjectAsync(project.Id);

            return iteration.Id;
        }

        public async Task<bool> TryPublishIteration(Guid iterationId)
        {
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = _trainingKey,
                Endpoint = _endpointUrl
            };

            var project = await trainingApi.GetProjectAsync(new Guid(_projectId));

            var iteration = await trainingApi.GetIterationAsync(project.Id, iterationId);

            if (iteration.Status != "Training")
            {
                await trainingApi.PublishIterationAsync(project.Id, iteration.Id, PublishedModelName, _predictionId);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}