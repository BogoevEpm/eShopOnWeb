namespace Microsoft.eShopWeb.Infrastructure;

public class PostOrderServiceSettings
{
    public string DeliveryProcessorEndpoint { get; set; }
    public string OrderItemsReserverServiceBusEndpoint { get; set; }
    public string OrderedItemsReserverServiceBusQueueName { get; set; }
}
