using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Communication.Events;
using SharedKernel.Utils;

namespace SharedKernel.Communication.Extensions;

public static class MassTransitServicesExtensions
{
    public static IServiceCollection AddMassTransit(
        this IServiceCollection services,
        IConfiguration config,
        Assembly consumersAssembly,
        string applicationName
    )
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumers(consumersAssembly);

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

                    configurator.Message<UserConfirmedEmailEvent>(e =>
                    {
                        e.SetEntityName("user-confirmed-email-event");
                    });
                    configurator.Publish<UserConfirmedEmailEvent>(e =>
                    {
                        e.ExchangeType = "fanout";
                        e.Durable = true;
                    });

                    configurator.ReceiveEndpoint(
                        $"user-confirmed-email-{applicationName}",
                        e =>
                        {
                            e.Durable = true;
                            e.AutoDelete = false;
                            
                            e.ConfigureConsumers(context);

                            e.Bind(
                                "user-confirmed-email-event",
                                b =>
                                {
                                    b.ExchangeType = "fanout";
                                    b.Durable = true;
                                }
                            );
                        }
                    );
                }
            );
        });

        return services;
    }
}
