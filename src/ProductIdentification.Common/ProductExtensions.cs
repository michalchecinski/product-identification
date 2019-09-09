using ProductIdentification.Core.DomainModels;

namespace ProductIdentification.Common
{
    public static class ProductExtensions
    {
        public static string StoragePath(this Product product)
        {
            return $"{product.Category.Name}/{product.SubCategory.Name}/{product.Name}".Replace(" ", "-");
        }

        public static string StoragePathOriginal(this Product product) => $"{product.StoragePath()}/original";

        public static string StoragePathToVerify(this Product product) => $"{product.StoragePath()}/to-verify";
        public static string StoragePathVerified(this Product product) => $"{product.StoragePath()}/verified";
    }
}