using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;

namespace Infrastructure.DIExtensions;

public static class ResilienceExtensions
{
    public static IServiceCollection AddResilience(this IServiceCollection services)
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
