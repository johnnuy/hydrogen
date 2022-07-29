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
    public class OrderStartedServiceBusFunction
    {

        const string OrderStartedTopic = "hydrogen-order-started";
        const string OrderStartedSubscription = "responder";
        readonly IMessageReceiver _receiver;

        public OrderStartedServiceBusFunction(IMessageReceiver receiver)
        {
            _receiver = receiver;
        }


        [FunctionName("OrderStarted")]
        public Task Start([ServiceBusTrigger(OrderStartedTopic, OrderStartedSubscription)] ServiceBusReceivedMessage message, CancellationToken cancellationToken, ILogger log)
        {
            return _receiver.HandleConsumer<OrderStartedConsumer>(OrderStartedTopic, message, cancellationToken);
        }
    }
}
