using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Repositories;

namespace ProductIdentification.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductIdentificationContext _context;

        public ProductRepository(ProductIdentificationContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(x => x.SubCategory)
                                 .Include(x => x.Category)
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var update = _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return update.Entity;
        }

        public Task<List<Product>> GetAll()
        {
            return _context.Products
                           .Include(x => x.SubCategory)
                           .Include(x => x.Category)
                           .ToListAsync();
        }

        public async Task<List<Product>> GetAll(Category category)
        {
            return await _context.Products
                                 .Where(x => x.Category.Id == category.Id)
                                 .Include(x => x.SubCategory)
                                 .Include(x => x.Category)
                                 .ToListAsync();
        }

        public async Task<List<Product>> GetAll(SubCategory subcategory)
        {
            return await _context.Products
                                 .Where(x => x.SubCategory.Id == subcategory.Id)
                                 .Include(x => x.SubCategory)
                                 .Include(x => x.Category)
                                 .ToListAsync();
        }

        public async Task<List<Product>> GetAllBySubCategoryId(int categoryId)
        {
            return await _context.Products
                                 .Where(x => x.Category.Id == categoryId)
                                 .Include(x => x.SubCategory)
                                 .Include(x => x.Category)
                                 .ToListAsync();
        }

        public async Task<List<Product>> GetAllByCategoryId(int subcategoryId)
        {
            return await _context.Products
                                 .Where(x => x.SubCategory.Id == subcategoryId)
                                 .Include(x => x.SubCategory)
                                 .Include(x => x.Category)
                                 .ToListAsync();
        }

        public async Task<Product> Get(int id)
        {
            return await _context.Products
                                 .Where(x => x.Id == id)
                                 .Include(x => x.Category)
                                 .Include(x => x.SubCategory)
                                 .FirstOrDefaultAsync();
        }
    }
}