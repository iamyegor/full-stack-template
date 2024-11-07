using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Auth;

public static class AuthServicesExtensions
{
    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddTransient<JwtClaims>();
        services.AddTransient<JwtValidationParameters>();
        services.Configure<JwtSettings>(config.GetSection(nameof(JwtSettings)));

        services
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                ServiceProvider serviceProvider = services.BuildServiceProvider();
                JwtValidationParameters validationParameters =
                    serviceProvider.GetRequiredService<JwtValidationParameters>();

                options.TokenValidationParameters = validationParameters.GetParameters();
                options.Events = TokenFromCookieExtractor.Extract();
            });

        services.AddAuthorization(AuthorizationPolicies.AddPolicies);

        return services;
    }
}
