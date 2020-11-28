using ProductIdentification.Core.DomainModels;

namespace ProductIdentification.Common
{
    public static class ProductExtensions
    {
        private static string StoragePath(this Product product)
        {
            return $"{product.Category.Name}/{product.SubCategory.Name}/{product.Name}"
                .Replace(" ", "-")
                .ToLowerInvariant();
        }

        public static string StoragePathOriginal(this Product product) => $"{product.StoragePath()}/original";

        public static string StoragePathToVerify(this Product product) => $"{product.StoragePath()}/to-verify";
        public static string StoragePathVerified(this Product product) => $"{product.StoragePath()}/verified";
        
        public static string StoragePathAddedAfterVerification(this Product product) => $"{product.StoragePath()}/added-verified";
    }
}