using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Data
{
    public class ProductIdentificationContext : IdentityDbContext
    {
        public ProductIdentificationContext(DbContextOptions<ProductIdentificationContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
    }
}