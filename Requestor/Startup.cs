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
                .AddScoped<StartOrderHttpFunction>()
                .AddScoped<StartOrderServiceBusFunction>()                
                .AddMassTransitForAzureFunctions(cfg =>
                    {
                        cfg.AddConsumer<StartOrderConsumer>();
                        cfg.AddConsumer<AsyncResponseConsumer>();
                        cfg.AddRequestClient<StartOrder>(new Uri("queue:hydrogen-start-order"));
                    },
                    "ConnectionStrings:AzureWebJobsServiceBus");
        }
    }
}