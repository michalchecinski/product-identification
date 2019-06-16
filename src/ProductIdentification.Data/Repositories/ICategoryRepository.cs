using System.Threading.Tasks;

namespace ProductIdentification.Data.Repositories
{
    using ProductIdentification.Core.Models;

    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
    }
}
