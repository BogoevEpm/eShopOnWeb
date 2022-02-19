using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace Microsoft.eShopWeb.ApplicationCore.DTO;

public class OrderDto
{
    public OrderDto(IEnumerable<OrderItemDto> orderedItems, Address shippingAddress, string buyerId)
    {
        if (!orderedItems.Any())
        {
            throw new ArgumentException(nameof(orderedItems));
        }

        BuyerId = buyerId;
        OrderedItems = orderedItems;
        Total = OrderedItems.Sum(item => item.UnitPrice);
        ShippingAddress = shippingAddress;
    }

    public IEnumerable<OrderItemDto> OrderedItems { get; }
    public decimal Total { get; }
    public Address ShippingAddress { get; }
    public string BuyerId { get; }
}
