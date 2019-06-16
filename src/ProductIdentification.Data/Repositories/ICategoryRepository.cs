namespace ProductIdentification.Data.Repositories
{
    using ProductIdentification.Core.Models;

    public interface ICategoryRepository
    {
        Category GetCategoryById(int id);
        void AddCategory(Category category);
        Category UpdateCategory(Category category);
    }
}
