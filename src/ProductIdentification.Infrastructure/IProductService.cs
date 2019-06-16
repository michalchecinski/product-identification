using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Infrastructure
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<List<Product>> GetAllBySubCategory(int subCategoryId);
        Task<List<Product>> GetAllByCategory(int categoryId);
        Task AddProduct(Product product);
        Task<Product> UpdateProduct(Product product);
    }
}
