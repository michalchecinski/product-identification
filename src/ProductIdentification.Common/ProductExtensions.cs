using ProductIdentification.Core.DomainModels;

namespace ProductIdentification.Common
{
    public static class ProductExtensions
    {
        public static string StoragePath(this Product product)
        {
            return $"{product.Category.Name}/{product.SubCategory.Name}/{product.Name}".Replace(" ", "-");
        }
        
        public static string StoragePathOriginal(this Product product)
        {
            return $"{product.StoragePath()}/original";
        }
    }
}