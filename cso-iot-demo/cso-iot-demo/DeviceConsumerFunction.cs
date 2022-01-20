using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace cso_iot_demo
{
    public class DeviceConsumerFunction
    {
        private readonly IRepairItemRepository _repairItemRepository;

        public DeviceConsumerFunction(IRepairItemRepository repairItemRepository)
        {
            _repairItemRepository = repairItemRepository;
        }

        [FunctionName(nameof(DeviceConsumerFunction))]
        public async Task Run([ServiceBusTrigger("%DeviceQueueName%", Connection = "ConnectionStrings:DeviceServiceBus")]string deviceMessage, ILogger log)
        {
            log.LogInformation($"Processing Device Message: {deviceMessage}");
            var test  = deviceMessage.Split('\"');
            var test2 = test[1].Split(":");
            int.TryParse(test2[0], out var repairOrderId);
            int.TryParse(test2[1], out var repairItemId);
            await _repairItemRepository.CompleteRepairItem(repairOrderId, repairItemId);
        }
    }
}
