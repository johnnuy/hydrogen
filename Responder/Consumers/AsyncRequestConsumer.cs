using System;
using System.Threading.Tasks;
using MassTransit;
using Hydrogen.Common;

namespace Hydrogen.Consumers
{
    public class AsyncRequestConsumer :IConsumer<AsyncRequest>
    {
        public async Task Consume(ConsumeContext<AsyncRequest> context)
        {
            LogContext.Info?.Log("Async Request Received: {request}", context.Message.Request);

            String responseAddress = context.Headers.Get<String>("ReplyTo");
            LogContext.Info?.Log("Responding to address {response}", responseAddress);

            ISendEndpoint queue = await context.GetSendEndpoint(new Uri(responseAddress));

            if (new Random().NextDouble() > 0.8) {
                await queue.Send<AsyncResponse>(new()
                {
                    Response = $"{context.Message.Request} -- Yes"
                });
            }
            else
            {
                await queue.Send<AsyncResponse>(new()
                {
                    Response = $"{context.Message.Request} -- No"
                });
            }
        }
    }
}