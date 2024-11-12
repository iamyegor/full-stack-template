using Infrastructure.Data.Dapper;
using Infrastructure.Data.Helpers;
using Infrastructure.Features.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DIExtensions;

public static class UtilsExtensions
{
    public static IServiceCollection AddUtils(this IServiceCollection services)
    {
        services
            .AddTransient<UserRemover>()
            .AddTransient<DapperConnectionFactory>()
            .AddTransient<HttpClient>()
            .AddTransient<ConnectionStringResolver>();

        return services;
    }
}
