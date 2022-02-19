using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Azure.Storage.Blobs;
using System.Collections.Generic;
using System.Text;
using Microsoft.eShopWeb.ApplicationCore.DTO;

namespace OrderItemsReserver
{
    public static class OrderItemsReserverFunction
    {
        private static string ContainerName = Environment.GetEnvironmentVariable("BlobContainerNaime");
        private static string ConnectionString = Environment.GetEnvironmentVariable("BlobConnectionString");

        [FunctionName("OrderItemsReserverFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var orderRequestJson = await new StreamReader(req.Body).ReadToEndAsync();
            var orderDetails = JsonConvert.DeserializeObject<OrderDto>(orderRequestJson);

            BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient($"{DateTime.UtcNow.Ticks}-{orderDetails.BuyerId}.json");

            List<dynamic> thinOrderJson = new List<dynamic>(orderDetails.OrderedItems.Select(item => new { ItemId = item.ProductId, Quantity = item.Quantity }));
            await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(thinOrderJson))));

            return new OkResult();
        }
    }
}
