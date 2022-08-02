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
using Hydrogen.Common;
using Newtonsoft.Json;
using System.Text;

namespace Hydrogen
{
    public class CancelOrderServiceBusFunction
    {

        const string CancelOrderQueueName = "hydrogen-cancel-order";
        readonly IMessageReceiver _receiver;

        public CancelOrderServiceBusFunction(IMessageReceiver receiver)
        {
            _receiver = receiver;
        }

        [FunctionName("CancelOrder")]
        public Task CancelOrder([ServiceBusTrigger(CancelOrderQueueName)] ServiceBusReceivedMessage message, CancellationToken cancellationToken, ILogger log)
        {            
            return _receiver.HandleConsumer<CancelOrderConsumer>(CancelOrderQueueName, message, cancellationToken);
        }
    }
}
