using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProductIdentification.Data.RepositoriesImplementations
{
    using System;
    using ProductIdentification.Core.Models;
    using ProductIdentification.Data.Repositories;

    public class ProductRepository : IProductRepository
    {
        private readonly ProductIdentificationContext _context;

        public ProductRepository(ProductIdentificationContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
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
    }
}
