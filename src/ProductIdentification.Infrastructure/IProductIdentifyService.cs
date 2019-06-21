using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Infrastructure
{
    public interface IProductIdentifyService
    {
        Task<Product> IdentifyProduct(Stream image);

        Task<Product> AddProduct(List<IFormFile> images, Product product);
    }
}