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
                    if (AppEnv.IsDevelopment)
                    {
                        var configService = context.GetRequiredService<IConfiguration>();
                        var connectionString = configService.GetConnectionString("rabbitmq");
                        configurator.Host(connectionString);
                    }
                    else
                    {
                        string host = Environment.GetEnvironmentVariable("RABBITMQ_HOST")!;
                        string username = Environment.GetEnvironmentVariable("RABBITMQ_USER")!;
                        string password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")!;
                        configurator.Host(
                            new Uri(host),
                            hostConfigurator =>
                            {
                                hostConfigurator.Username(username);
                                hostConfigurator.Password(password);
                            }
                        );
                    }

                    configurator.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));

                    configurator.ConfigureEndpoints(context);
                }
            );
        });

        return services;
    }
}
