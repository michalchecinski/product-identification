using System.Collections.Generic;
using System.Threading.Tasks;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Infrastructure
{
    public interface ICategoryService
    {
        Task<Category> GetCategory(int id);
        Task AddCategory(Category category);
        Task<Category> UpdateCategory(Category category);
        Task<List<Category>> GetAllCategories();
        Task<List<string>> GetAllCategoriesNames();
        Task<string> GetCategoryNameById(int id);
    }
}
