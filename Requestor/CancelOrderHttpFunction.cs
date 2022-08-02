using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MassTransit;
using MassTransit.Serialization;
using System.Text.Json;
using Hydrogen.Common;

namespace Hydrogen
{
    public class CancelOrderHttpFunction
    {

        readonly IAsyncBusHandle _handle;
        readonly IRequestClient<CancelOrder> _client;

        public CancelOrderHttpFunction(IAsyncBusHandle handle, IRequestClient<CancelOrder> client)
        {
            _handle = handle;
            _client = client;
        }


        [FunctionName("CancelOrderHttp")]
        public async Task<IActionResult> CancelOrder([HttpTrigger(AuthorizationLevel.Function, "post", Route = "cancel")] HttpRequest request, ILogger log)
        {            
            log.LogInformation("CancelOrder invoked via HTTP Trigger");
            var body = await request.ReadAsStringAsync();
            var order = JsonSerializer.Deserialize<CancelOrder>(body, SystemTextJsonMessageSerializer.Options);
            if (order == null)
            {
                return new BadRequestResult();
            }

            var response = await _client.GetResponse<OrderCancelled>(order);

            return new OkObjectResult(new
            {
                response.Message.Id
            });
        }
    }
}
