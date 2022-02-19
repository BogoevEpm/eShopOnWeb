using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.DTO;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Options;

namespace Microsoft.eShopWeb.Infrastructure.Services;

public class HttpDeliveryOrderProcessorService : IDeliveryOrderProcessorService
{
    // Replace with settings
    private readonly PostOrderServiceSettings _settings;
    public HttpDeliveryOrderProcessorService(IOptions<PostOrderServiceSettings> settings)
    {
        _settings = settings.Value;
    }
    public async Task ProcessOrderAsync(OrderDto order)
    {
        // TODO : handle response ?

        using(var httpClient = new HttpClient())
        {
            var processOrderRequest = new HttpRequestMessage(HttpMethod.Post, _settings.DeliveryProcessorEndpoint)
            {
                Content = JsonContent.Create(order)
            };

            var processOrderResponse = await httpClient.SendAsync(processOrderRequest);
            processOrderResponse.EnsureSuccessStatusCode();
        }
    }
}
