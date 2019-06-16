namespace ProductIdentification.Data.Repositories
{
    using ProductIdentification.Core.Models;

    public interface ISubCategoryRepository
    {
        SubCategory GetProductById(int id);
        void AddProduct(SubCategory subCategory);
        SubCategory UpdateProduct(SubCategory subCategory);
    }
}
