using System;
using System.Threading.Tasks;
using MassTransit;
using Hydrogen.Common;

namespace Hydrogen.Consumers
{
    public class StartOrderConsumer :IConsumer<StartOrder>
    {
        public async Task Consume(ConsumeContext<StartOrder> context)
        {
            LogContext.Debug?.Log("Processing Order: {OrderNumber}", context.Message.OrderNumber);

            OrderStarted orderStarted = new OrderStarted()
            {
                Id = context.Message.Id,
                OrderNumber = context.Message.OrderNumber
            };

            ISendEndpoint topic = await context.GetSendEndpoint(new Uri("topic:hydrogen-order-started"));
            await topic.Send<OrderStarted>(orderStarted);

            ISendEndpoint queue = await context.GetSendEndpoint(new Uri("queue:hydrogen-async-requests"));
            await queue.Send<AsyncRequest>(new()
            {
                Request = $"Can I proceed with order {context.Message.OrderNumber}?"
            });

            await context.RespondAsync<OrderStarted>(orderStarted);
        }
    }
}