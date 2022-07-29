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
    public class AsyncResponseServiceBusFunction
    {

        const string AsyncResponseQueueName = "hydrogen-async-responses";
        readonly IMessageReceiver _receiver;

        public AsyncResponseServiceBusFunction(IMessageReceiver receiver)
        {
            _receiver = receiver;
        }


        [FunctionName("AsyncResponse")]
        public Task Start([ServiceBusTrigger(AsyncResponseQueueName)] ServiceBusReceivedMessage message, CancellationToken cancellationToken, ILogger log)
        {
            return _receiver.HandleConsumer<AsyncResponseConsumer>(AsyncResponseQueueName, message, cancellationToken);
        }
    }
}
