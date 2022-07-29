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
    public class StartOrderHttpFunction
    {

        readonly IAsyncBusHandle _handle;
        readonly IRequestClient<StartOrder> _client;

        public StartOrderHttpFunction(IAsyncBusHandle handle, IRequestClient<StartOrder> client)
        {
            _handle = handle;
            _client = client;
        }


        [FunctionName("InitiateStartOrder")]
        public async Task<IActionResult> Start([HttpTrigger(AuthorizationLevel.Function, "post", Route = "start")] HttpRequest request, ILogger log)
        {            
            log.LogInformation("InitiateStartOrder invoked via HTTP Trigger");
            var body = await request.ReadAsStringAsync();
            var order = JsonSerializer.Deserialize<StartOrder>(body, SystemTextJsonMessageSerializer.Options);
            if (order == null)
            {
                return new BadRequestResult();
            }

            var response = await _client.GetResponse<OrderStarted>(order);

            return new OkObjectResult(new
            {
                response.Message.Id
            });
        }
    }
}
