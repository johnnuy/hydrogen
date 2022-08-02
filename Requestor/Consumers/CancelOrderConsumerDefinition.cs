using MassTransit;

namespace Hydrogen.Consumers
{
    public class CancelOrderConsumerDefinition : ConsumerDefinition<CancelOrderConsumer>
    {
        protected override void ConfigureConsumer(
            IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<CancelOrderConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(x => x.Intervals(10, 100, 500, 1000));
        }
    }
}