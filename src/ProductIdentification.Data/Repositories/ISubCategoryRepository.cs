namespace ProductIdentification.Data.Repositories
{
    using ProductIdentification.Core.Models;
    using System.Threading.Tasks;

    public interface ISubCategoryRepository
    {
        Task<SubCategory> GetSubCategoryByIdAsync(int id);
        Task AddSubCategoryAsync(SubCategory subCategory);
        Task<SubCategory> UpdateSubCategoryAsync(SubCategory subCategory);
    }
}
