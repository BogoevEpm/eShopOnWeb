using System;
using Microsoft.eShopWeb.ApplicationCore.DTO;
using Newtonsoft.Json;

namespace DeliveryOrderProcessor;

internal class OrderDeliveryEntity
{
    public OrderDeliveryEntity(OrderDto order)
    {
        Order = order;
    }

    [JsonProperty("id")]
    public string Id { get; } = Guid.NewGuid().ToString();
    public OrderDto Order { get; }
}
