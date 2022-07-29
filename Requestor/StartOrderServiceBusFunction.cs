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
    public class StartOrderServiceBusFunction
    {

        const string StartOrderQueueName = "hydrogen-start-order";
        readonly IMessageReceiver _receiver;

        public StartOrderServiceBusFunction(IMessageReceiver receiver)
        {
            _receiver = receiver;
        }


        [FunctionName("StartOrder")]
        public Task Start([ServiceBusTrigger(StartOrderQueueName)] ServiceBusReceivedMessage message, CancellationToken cancellationToken, ILogger log)
        {
            return _receiver.HandleConsumer<StartOrderConsumer>(StartOrderQueueName, message, cancellationToken);
        }
    }
}
