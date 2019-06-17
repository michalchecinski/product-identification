using System.Collections.Generic;
using System.Threading.Tasks;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<List<Product>> GetAll();
        Task<List<Product>> GetAllBySubCategoryId(int categoryId);
        Task<List<Product>> GetAllByCategoryId(int subcategoryId);
        Task<Product> Get(int id);
    }
}
