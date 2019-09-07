namespace ProductIdentification.Core.Models.Messages
{
    public class AddProductMessage : IQueueMessage
    {
        public AddProductMessage(int productId)
        {
            ProductId = productId;
        }
        
        public int ProductId { get; set; }
    }
}