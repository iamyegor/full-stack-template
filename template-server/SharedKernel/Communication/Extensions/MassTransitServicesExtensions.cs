using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                    string username = config["RabbitMq:Username"]!;
                    string password = config["RabbitMq:Password"]!;

                    configurator.Host(
                        new Uri(config["RabbitMq:Host"]!),
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
