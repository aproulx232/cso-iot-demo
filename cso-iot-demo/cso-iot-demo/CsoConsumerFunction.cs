using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cso.Contracts.NotificationServices.InventoryDataMessages;
using Microsoft.Azure.Devices;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace cso_iot_demo
{
    public class CsoConsumerFunction
    {
        private static ServiceClient _serviceClient;
        private ILogger _logger;
        private const string ConnectionString = "HostName=allen-test.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=dE8jIoFVT43VIFPKDFtpb87ElWvxGNSV0vrJ1Ha2wXw=";
        private const string TargetDevice = "myDeviceId";

        private const string RepairMessageType = "Repairs";

        [FunctionName(nameof(CsoConsumerFunction))]
        public async Task Run([ServiceBusTrigger("%CsoQueueName%", Connection = "ConnectionStrings:CsoServiceBus")]string message, ILogger log)
        {
            _logger = log;
            _logger.LogInformation($"Processing CSO message: {message}");

            var inventoryDataMessage = JsonConvert.DeserializeObject<InventoryDataMessage>(message);
            if (!inventoryDataMessage.InventoryData.InventoryID.HasValue)
            {
                _logger.LogWarning("CSO message does not contain InventoryId");
                return;
            }

            switch (inventoryDataMessage.Header.MessageType)
            {
                case RepairMessageType:
                    await ProcessRepairsMessage(inventoryDataMessage);
                    break;
                default:
                    _logger.LogInformation(
                        $"Message of type {inventoryDataMessage.Header.MessageType} is not processed");
                    return;
            }
        }

        private async Task ProcessRepairsMessage(InventoryDataMessage inventoryDataMessage)
        {
            var repairItems = inventoryDataMessage.InventoryData.Repairs
                .SelectMany(r => r.RepairItems, (ro, ri) => new { ro, ri }).Where(roandri => roandri.ri.CompleteFlag != "Y").Select(roandi =>
                    $"{roandi.ro.RepairOrderDetails.RepairOrderId}: {roandi.ri.RepairItemId}: {roandi.ri.Action}: {roandi.ri.Comment}");
            await SendCloudToDeviceMessage(JsonConvert.SerializeObject(repairItems));
        }

        private async Task SendCloudToDeviceMessage(string message)
        {
            _logger.LogInformation($"Send Cloud-to-Device message: {message}");
            _serviceClient = ServiceClient.CreateFromConnectionString(ConnectionString);
            var commandMessage = new
                Message(Encoding.ASCII.GetBytes(message));
            await _serviceClient.SendAsync(TargetDevice, commandMessage);
        }
    }
}
