using System.Net;
using System.Net.Mail;
using Infrastructure.Auth.Authentication;
using Infrastructure.Auth.VkAuth;
using Infrastructure.Cookies;
using Infrastructure.Data;
using Infrastructure.Data.Dapper;
using Infrastructure.Emails;
using Infrastructure.Extensions;
using Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            .AddMassTransit(config);

        services.AddAuth(config).AddTransient<TokensGenerator>();

        return services;
    }

    private static IServiceCollection AddUtils(this IServiceCollection services)
    {
        services
            .AddTransient<UserIdExtractor>()
            .AddTransient<VkAuthTokenManager>()
            .AddTransient<UserRemover>()
            .AddTransient<DapperConnectionFactory>()
            .AddTransient<HttpClient>()
            .AddTransient<ConnectionStringResolver>();

        return services;
    }

    private static IServiceCollection AddEmails(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.Configure<EmailSettings>(config.GetSection(nameof(EmailSettings)));
        services.AddTransient<DomainEmailSender>();
        services.AddTransient<EmailSender>();
        services.AddTransient(serviceProvider =>
        {
            EmailSettings emailSettings = serviceProvider
                .GetRequiredService<IOptions<EmailSettings>>()
                .Value;

            return new SmtpClient(emailSettings.MailServer, emailSettings.MailPort)
            {
                Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password),
                EnableSsl = true
            };
        });

        return services;
    }
}
