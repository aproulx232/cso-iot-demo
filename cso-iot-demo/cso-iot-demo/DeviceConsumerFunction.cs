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
            deviceMessage = "[\"202741038: 1: PSI 7 Day: \"]";

            await _repairItemRepository.CompleteRepairItem(202741039, 1, 19, 13);
        }
    }
}
