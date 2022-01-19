using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace cso_iot_demo
{
    public class DeviceConsumerFunction
    {
        [FunctionName(nameof(DeviceConsumerFunction))]
        public void Run([ServiceBusTrigger("%DeviceQueueName%", Connection = "ConnectionStrings:DeviceServiceBus")]string deviceMessage, ILogger log)
        {
            log.LogInformation($"Processing Device Message: {deviceMessage}");
        }
    }
}
