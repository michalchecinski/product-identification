namespace ProductIdentification.Data.RepositoriesImplementations
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ProductIdentification.Core.Models;
    using ProductIdentification.Data.Repositories;

    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly ProductIdentificationContext _context;

        public SubCategoryRepository(ProductIdentificationContext context)
        {
            _context = context;
        }

        public async Task<SubCategory> GetSubCategoryByIdAsync(int id)
        {
            return await _context.SubCategories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddSubCategoryAsync(SubCategory subCategory)
        {
            await _context.SubCategories.AddAsync(subCategory);
            await _context.SaveChangesAsync();
        }

        public async Task<SubCategory> UpdateSubCategoryAsync(SubCategory subCategory)
        {
            var update = _context.SubCategories.Update(subCategory);
            await _context.SaveChangesAsync();
            return update.Entity;
        }
    }
}