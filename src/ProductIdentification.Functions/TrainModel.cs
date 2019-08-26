using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ProductIdentification.Functions
{
    public class TrainModel
    {
        [FunctionName("TrainModel")]
        public void Run([QueueTrigger("train-model", Connection = "Storage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
