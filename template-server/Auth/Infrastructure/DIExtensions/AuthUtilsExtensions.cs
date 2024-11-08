using Infrastructure.Features.Auth;
using Infrastructure.Features.Auth.Google;
using Infrastructure.Features.Auth.Vk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DIExtensions;

public static class AuthUtilsExtensions
{
    public static IServiceCollection AddAuthUtils(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services
            .AddTransient<GoogleIdTokenValidator>()
            .AddTransient<UserIdExtractor>()
            .AddTransient<VkAuthTokenManager>()
            .AddTransient<TokensGenerator>();

        return services;
    }
}
