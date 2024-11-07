using Infrastructure.Data;
using Infrastructure.Data.Dapper;
using Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Auth;

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
            .AddTransient<DapperConnectionFactory>()
            .AddTransient<HttpClient>()
            .AddTransient<ConnectionStringResolver>()
            .AddAuth(config);

        return services;
    }
}
