using System.Reflection;
using MassTransit;
using SharedKernel.Utils;

namespace Api.DiExtensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransit(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddMassTransit(busConfig =>
        {
            busConfig.SetKebabCaseEndpointNameFormatter();

            busConfig.AddConsumers(Assembly.GetExecutingAssembly());

            busConfig.UsingRabbitMq(
                (context, cfg) =>
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

                    cfg.Host(
                        new Uri(host),
                        hostConfigurator =>
                        {
                            hostConfigurator.Username(username);
                            hostConfigurator.Password(password);
                        }
                    );

                    cfg.ReceiveEndpoint(
                        "app-queue",
                        e =>
                        {
                            e.ConfigureConsumers(context);
                        }
                    );
                }
            );
        });

        return services;
    }
}
