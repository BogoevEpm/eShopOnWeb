using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.eShopWeb.ApplicationCore.DTO;
using Microsoft.Azure.Cosmos;

namespace DeliveryOrderProcessor
{
    public static class DeliveryOrderProcessor
    {
        private static string ContainerId = System.Environment.GetEnvironmentVariable("DeliveryOrderProcessorContainer");
        private static string DatabaseId = System.Environment.GetEnvironmentVariable("DeliveryOrderProcessorDatabase");
        private static string PrimaryKey = System.Environment.GetEnvironmentVariable("DeliveryOrderProcessorDatabasePrimaryKey");
        private static string EndpointUrl = System.Environment.GetEnvironmentVariable("DeliveryOrderProcessorDatabaseEndpoint");

        [FunctionName("DeliveryOrderProcessor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                OrderDto orderDetails = JsonConvert.DeserializeObject<OrderDto>(requestBody);
                var orderDeliveryEntity = new OrderDeliveryEntity(orderDetails);

                var container = await GetContainer();
                await container.CreateItemAsync(orderDeliveryEntity, new PartitionKey(orderDeliveryEntity.Order.BuyerId));
                return new OkResult();

            }
            catch
            {

                return new BadRequestResult();
            }
        }

        private static async Task<Container> GetContainer()
        {
            var cosmosClient = new CosmosClient(EndpointUrl, PrimaryKey);
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
            return await database.Database.CreateContainerIfNotExistsAsync(ContainerId, "/Order/BuyerId");
        }
    }
}
