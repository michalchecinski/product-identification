namespace ProductIdentification.Core.Models
{
    using Microsoft.Azure.Cosmos.Table;
    
    public class ProductTrainingModel: TableEntity
    {
        public ProductTrainingModel()
        {
            
        }
        
        public ProductTrainingModel(int productId, string trainingState = TrainingStates.ToTrain)
        {
            PartitionKey = trainingState;
            RowKey = productId.ToString();
        }

        public int ProductId => int.Parse(RowKey);

        public string TrainingState
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }
    }
}