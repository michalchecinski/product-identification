using System.Collections.Generic;
using System.Threading.Tasks;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Infrastructure
{
    public interface ISubCategoryService
    {
        Task<List<SubCategory>> GetAllByCategory(int categoryId);
        Task<SubCategory> GetSubcategoryById(int id);
        Task AddSubcategory(SubCategory subCategory);
        Task<SubCategory> AddSubcategory(string subCategoryName, string categoryName);
        Task<SubCategory> UpdateSubcategory(SubCategory subCategory);
        Task<SubCategory> UpdateSubcategory(string subCategoryName, string categoryName);
        Task<IEnumerable<SubCategory>> GetSubcategories();
        Task<string> GetSubcategoryNameById(int id);
    }
}