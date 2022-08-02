using System;
using System.Threading.Tasks;
using MassTransit;
using Hydrogen.Common;

namespace Hydrogen.Consumers
{
    public class CancelOrderConsumer :IConsumer<CancelOrder>
    {
        public async Task Consume(ConsumeContext<CancelOrder> context)
        {
            LogContext.Debug?.Log("Cancelling Order: {OrderNumber}", context.Message.OrderNumber);

            OrderCancelled orderCancelled = new OrderCancelled()
            {
                Id = context.Message.Id,
                OrderNumber = context.Message.OrderNumber
            };

            await context.RespondAsync<OrderCancelled>(orderCancelled);
        }
    }
}