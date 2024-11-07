using Infrastructure.Data;
using Infrastructure.Data.Dapper;
using Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
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
            .AddResilience()
            .AddAuth(config);

        return services;
    }

    private static IServiceCollection AddResilience(this IServiceCollection services)
    {
        services.AddResiliencePipeline(
            "db",
            x =>
            {
                x.AddRetry(
                    new RetryStrategyOptions()
                    {
                        ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                        Delay = TimeSpan.FromSeconds(5),
                        MaxRetryAttempts = 3,
                        BackoffType = DelayBackoffType.Constant
                    }
                );
            }
        );

        return services;
    }
}
