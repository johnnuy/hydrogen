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
using Azure.Messaging.ServiceBus;
using System.Threading;
using Hydrogen.Consumers;

namespace Hydrogen
{
    public class AsyncRequestServiceBusFunction
    {

        const string AsyncRequestQueue = "hydrogen-async-requests";
        readonly IMessageReceiver _receiver;

        public AsyncRequestServiceBusFunction(IMessageReceiver receiver)
        {
            _receiver = receiver;
        }


        [FunctionName("AsyncRequest")]
        public Task Request([ServiceBusTrigger(AsyncRequestQueue)] ServiceBusReceivedMessage message, CancellationToken cancellationToken, ILogger log)
        {
            return _receiver.HandleConsumer<AsyncRequestConsumer>(AsyncRequestQueue, message, cancellationToken);
        }
    }
}
