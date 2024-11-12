using Infrastructure.Data;
using Infrastructure.Data.Helpers;
using Infrastructure.DIExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Auth;
using SharedKernel.Communication.Extensions;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration config,
        bool isDevelopment
    )
    {
        ConnectionStringResolver connectionStringResolver = new ConnectionStringResolver(config);
        string connectionString = connectionStringResolver.GetBasedOnEnvironment();

        services
            .AddScoped(_ => new ApplicationContext(connectionString, isDevelopment))
            .AddUtils()
            .AddEmails(config)
            .AddResilience()
            .AddMassTransit(config)
            .AddAuth(config)
            .AddAuthUtils(config);

        return services;
    }
}
