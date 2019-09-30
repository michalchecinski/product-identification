using ProductIdentification.Infrastructure;

namespace ProductIdentification.Data
{
    public class AzureStorageAccountFactory
    {
        private readonly ISecretsFetcher _secretsFetcher;

        public AzureStorageAccountFactory(ISecretsFetcher secretsFetcher)
        {
            _secretsFetcher = secretsFetcher;
        }

        public Microsoft.Azure.Cosmos.Table.CloudStorageAccount GetTableStorageAccount()
        {
            return Microsoft.Azure.Cosmos.Table.CloudStorageAccount.Parse(_secretsFetcher.GetStorageConnectionString);
        }

        public Microsoft.Azure.Storage.CloudStorageAccount GetStorageAccount()
        {
            return Microsoft.Azure.Storage.CloudStorageAccount.Parse(_secretsFetcher.GetStorageConnectionString);
        }
    }
}