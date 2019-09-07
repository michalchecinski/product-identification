using ProductIdentification.Core.DomainModels;

namespace ProductIdentification.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ProductIdentificationContext : IdentityDbContext
    {
        public ProductIdentificationContext(DbContextOptions<ProductIdentificationContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Product>()
            //            .HasOne(p => p.Category)
            //            .WithMany(c => c.Products)
            //            .HasForeignKey(x => x.CategoryId)
            //            .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Product>()
            //            .HasIndex(p => p.CategoryId)
            //            .IsUnique(false);

            modelBuilder.Entity<Product>()
                        .HasOne(p => p.SubCategory)
                        .WithMany(s => s.Products)
                        .HasForeignKey(x => x.SubCategoryId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                        .HasIndex(p => p.SubCategoryId)
                        .IsUnique(false);

            modelBuilder.Entity<SubCategory>()
                        .HasOne(s => s.Category)
                        .WithMany(c => c.SubCategories)
                        .HasForeignKey(s => s.CategoryId)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}