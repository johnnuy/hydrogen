using Hydrogen;
using Hydrogen.Consumers;
using MassTransit;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]


namespace Hydrogen
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                .AddEnvironmentVariables()
                .Build();

            builder.Services
                .AddSingleton<IConfiguration>(config)
                .AddScoped<OrderStartedServiceBusFunction>()
                .AddScoped<AsyncRequestServiceBusFunction>()
                .AddMassTransitForAzureFunctions(cfg =>
                    {
                        cfg.AddConsumer<OrderStartedConsumer>();
                        cfg.AddConsumer<AsyncRequestConsumer>();                                                
                    },
                    "ConnectionStrings:AzureWebJobsServiceBus");
        }
    }
}