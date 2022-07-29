using System;
using System.Threading.Tasks;
using MassTransit;
using Hydrogen.Common;

namespace Hydrogen.Consumers
{
    public class AsyncResponseConsumer :IConsumer<AsyncResponse>
    {
        public async Task Consume(ConsumeContext<AsyncResponse> context)
        {
            LogContext.Info?.Log("Async Response Received: {request}", context.Message.Response);
        }
    }
}