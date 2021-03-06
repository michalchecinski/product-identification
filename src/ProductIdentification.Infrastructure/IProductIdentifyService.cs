﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProductIdentification.Core.DomainModels;
using ProductIdentification.Core.Dto;

namespace ProductIdentification.Infrastructure
{
    public interface IProductIdentifyService
    {
        Task<Product> IdentifyProduct(Stream image);

        Task<Product> AddProduct(List<Stream> images, Product product);

        Task<Guid> TrainProjectAsync();

        Task<bool> TryPublishIteration(Guid iterationId);
        Task UpdateProduct(List<Stream> images, Product product);
    }
}