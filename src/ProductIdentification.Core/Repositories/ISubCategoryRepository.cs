using System.Collections.Generic;
using System.Threading.Tasks;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Core.Repositories
{
    public interface ISubCategoryRepository
    {
        Task<SubCategory> GetSubCategoryByIdAsync(int id);
        Task AddSubCategoryAsync(SubCategory subCategory);
        Task<SubCategory> UpdateSubCategoryAsync(SubCategory subCategory);
        Task<List<SubCategory>> GetAll();
        Task<List<SubCategory>> GetAll(int categoryId);
        Task<string> GetName(int id);
        Task<List<string>> GetNamesByCategory(int categoryId);
    }
}
