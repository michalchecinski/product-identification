using System.Threading.Tasks;

namespace ProductIdentification.Data.Repositories
{
    using ProductIdentification.Core.Models;

    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
    }
}
