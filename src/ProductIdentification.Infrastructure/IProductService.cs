using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Infrastructure
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<List<Product>> GetAllBySubCategory(int subCategoryId);
        Task<List<Product>> GetAllByCategory(int categoryId);
        Task<Product> AddProduct(Product product, List<IFormFile> images);
        Task<Product> UpdateProduct(Product product);
        Task<List<Product>> GetAll();
        Task<Product> AddProduct(Product product, string categoryName, string subCategoryName,
            List<IFormFile> images);

        Task<Product> UpdateProduct(Product product, string categoryName, string subCategoryName);
    }
}
