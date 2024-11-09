using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Utils;

namespace SharedKernel.Communication.Extensions;

public static class MassTransitServicesExtensions
{
    public static IServiceCollection AddMassTransit(
        this IServiceCollection services,
        IConfiguration config,
        Assembly? assembly = null
    )
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
            {
                busConfigurator.AddConsumers(assembly);
            }

            busConfigurator.UsingRabbitMq(
                (context, configurator) =>
                {
                    string host = config["RabbitMq:Host"]!;
                    string username = config["RabbitMq:Username"]!;
                    string password = config["RabbitMq:Password"]!;
                    if (AppEnv.IsProduction)
                    {
                        host = Environment.GetEnvironmentVariable("RABBITMQ_HOST")!;
                        username = Environment.GetEnvironmentVariable("RABBITMQ_USER")!;
                        password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")!;
                    }

                    configurator.Host(
                        new Uri(host),
                        hostConfigurator =>
                        {
                            hostConfigurator.Username(username);
                            hostConfigurator.Password(password);
                        }
                    );

                    configurator.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));

                    configurator.ConfigureEndpoints(context);
                }
            );
        });

        return services;
    }
}
