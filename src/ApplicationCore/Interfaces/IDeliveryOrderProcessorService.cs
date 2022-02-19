using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.DTO;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IDeliveryOrderProcessorService
{
    Task ProcessOrderAsync(OrderDto order);
}
