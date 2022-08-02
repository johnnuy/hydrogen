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
        public override async void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                .AddEnvironmentVariables()
                .Build();

            IServiceCollection services = builder.Services
                .AddSingleton<IConfiguration>(config)
                .AddScoped<StartOrderHttpFunction>()
                .AddScoped<StartOrderServiceBusFunction>()
                .AddScoped<CancelOrderHttpFunction>()
                .AddScoped<CancelOrderServiceBusFunction>()
                .AddMassTransitForAzureFunctions(cfg =>
                    {
                        //cfg.AddConsumersFromNamespaceContaining<ConsumerNamespace>();
                        cfg.AddConsumer<StartOrderConsumer>();
                        cfg.AddConsumer<CancelOrderConsumer>();
                        cfg.AddConsumer<AsyncResponseConsumer>();
                        cfg.AddRequestClient<StartOrder>(new Uri("queue:hydrogen-start-order"));
                        cfg.AddRequestClient<CancelOrder>(new Uri("queue:hydrogen-cancel-order"));                       
                    },
                    "ConnectionStrings:AzureWebJobsServiceBus");
        }
    }
}