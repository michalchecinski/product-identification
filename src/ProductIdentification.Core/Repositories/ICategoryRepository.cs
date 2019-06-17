using System.Collections.Generic;
using System.Threading.Tasks;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Core.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<List<Category>> GetAll();
        Task<List<string>> GetAllNames();
        Task<Category> GetCategoryByNameAsync(string categoryName);
        Task<string> GetName(int id);
    }
}
