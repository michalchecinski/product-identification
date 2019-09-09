namespace ProductIdentification.Core.Models.Messages
{
    public class UpdateProductMessage : IQueueMessage
    {
        public UpdateProductMessage(int productId)
        {
            ProductId = productId;
        }
        
        public int ProductId { get; }
    }
}