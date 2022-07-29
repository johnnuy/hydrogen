using System;
using System.Threading.Tasks;
using MassTransit;
using Hydrogen.Common;

namespace Hydrogen.Consumers
{
    public class OrderStartedConsumer :IConsumer<OrderStarted>
    {
        public async Task Consume(ConsumeContext<OrderStarted> context)
        {
            LogContext.Info?.Log("Order Started: {OrderNumber}", context.Message.OrderNumber);
        }
    }
}