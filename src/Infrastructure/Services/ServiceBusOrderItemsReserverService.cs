using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.eShopWeb.ApplicationCore.DTO;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Microsoft.eShopWeb.Infrastructure.Services;

public class ServiceBusOrderItemsReserverService : IOrderItemsReserverService
{
    private readonly PostOrderServiceSettings _settings;

    public ServiceBusOrderItemsReserverService(IOptions<PostOrderServiceSettings> settings)
    {
        _settings = settings.Value;
    }
    public async Task ReserveItems(OrderDto order)
    {
        await using(var serviceBusClient = new ServiceBusClient(_settings.OrderItemsReserverServiceBusEndpoint))
        {
            var serviceBusSender = serviceBusClient.CreateSender(_settings.OrderedItemsReserverServiceBusQueueName);
            var orderMessage = new ServiceBusMessage(JsonConvert.SerializeObject(order));

            await serviceBusSender.SendMessageAsync(orderMessage);
        }
    }
}
