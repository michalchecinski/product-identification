namespace ProductIdentification.Data.Repositories
{
    using ProductIdentification.Core.Models;

    public interface IProductRepository
    {
        Product GetProductById(int id);
        void AddProduct(Product product);
        Product UpdateProduct(Product product);
    }
}
