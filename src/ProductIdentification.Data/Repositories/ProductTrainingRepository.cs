using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Repositories;
using CloudStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount;

namespace ProductIdentification.Data.Repositories
{
    public class ProductTrainingRepository : IProductTrainingRepository
    {
        private readonly CloudTable _table;
        private const string TableName = "ProductTrain";

        public ProductTrainingRepository(AzureStorageAccountFactory storageAccountFactory)
        {
            var storageAccount = storageAccountFactory.GetTableStorageAccount();
            var tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(TableName);
            _table.CreateIfNotExistsAsync();
        }

        public ProductTrainingModel Get(int productId)
        {
            var query = new TableQuery<ProductTrainingModel>()
                .Where(TableQuery.GenerateFilterCondition(
                    nameof(ProductTrainingModel.RowKey), 
                    QueryComparisons.Equal,
                    productId.ToString()))
                .Take(1);
            
            return _table.ExecuteQuery(query).FirstOrDefault();
        }

        public async Task<IEnumerable<ProductTrainingModel>> GetAllAsync()
        {
            TableContinuationToken token = null;
            var entities = new List<ProductTrainingModel>();
            do
            {
                var queryResult =
                    await _table.ExecuteQuerySegmentedAsync(new TableQuery<ProductTrainingModel>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities;
        }

        public IEnumerable<ProductTrainingModel> GetAllToTrain()
        {
            var query = new TableQuery<ProductTrainingModel>()
                    .Where(TableQuery.GenerateFilterCondition(
                        nameof(ProductTrainingModel.PartitionKey), 
                        QueryComparisons.Equal,
                        TrainingStates.ToTrain));
            
            return _table.ExecuteQuery(query);
        }
        
        public IEnumerable<ProductTrainingModel> GetAllTraining()
        {
            var query = new TableQuery<ProductTrainingModel>()
                .Where(TableQuery.GenerateFilterCondition(
                    nameof(ProductTrainingModel.PartitionKey), 
                    QueryComparisons.Equal,
                    TrainingStates.Training));
            
            return _table.ExecuteQuery(query);
        }

        public bool Any()
        {
            return _table.ExecuteQuery(new TableQuery<ProductTrainingModel>())
                         .ToList()
                         .Any();
        }
        
        public void Update(ProductTrainingModel productTrainingModel)
        {
            var operation = TableOperation.InsertOrMerge(productTrainingModel);
            _table.ExecuteAsync(operation);
        }
        
        public void Add(ProductTrainingModel productTrainingModel)
        {
            var operation = TableOperation.Insert(productTrainingModel);
            _table.ExecuteAsync(operation);
        }

        public void Delete(ProductTrainingModel productTrainingModel)
        {
            var operation = TableOperation.Delete(productTrainingModel);
            _table.ExecuteAsync(operation);
        }
    }
}