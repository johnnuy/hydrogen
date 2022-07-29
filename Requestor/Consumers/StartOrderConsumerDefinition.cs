using MassTransit;

namespace Hydrogen.Consumers
{
    public class StartOrderConsumerDefinition : ConsumerDefinition<StartOrderConsumer>
    {
        protected override void ConfigureConsumer(
            IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<StartOrderConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(x => x.Intervals(10, 100, 500, 1000));
        }
    }
}