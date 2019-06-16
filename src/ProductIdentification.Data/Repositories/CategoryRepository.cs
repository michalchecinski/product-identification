using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Repositories;

namespace ProductIdentification.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductIdentificationContext _context;

        public CategoryRepository(ProductIdentificationContext context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var update = _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return update.Entity;
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}