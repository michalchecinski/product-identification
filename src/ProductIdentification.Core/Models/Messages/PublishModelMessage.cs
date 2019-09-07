using System;

namespace ProductIdentification.Core.Models.Messages
{
    public class PublishModelMessage : IQueueMessage
    {
        public PublishModelMessage(Guid iterationId)
        {
            IterationId = iterationId;
        }
        
        public Guid IterationId { get; }
    }
}