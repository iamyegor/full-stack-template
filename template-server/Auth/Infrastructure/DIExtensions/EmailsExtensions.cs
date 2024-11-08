using Infrastructure.Features.Emails;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DIExtensions;

public static class EmailsExtensions
{
    public static IServiceCollection AddEmails(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.Configure<EmailSettings>(config.GetSection(nameof(EmailSettings)));
        services.AddTransient<DomainEmailSender>();
        services.AddTransient<EmailSender>();

        return services;
    }
}
