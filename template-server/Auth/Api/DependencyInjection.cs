using System.Net;
using Api.Mappings;
using MailKit.Security;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;
using SharedKernel.Utils;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddBaseServices(
        this IServiceCollection services,
        string corsPolicy
    )
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(corsPolicy);
        services.RegisterMappings();

        return services;
    }

    private static void AddCors(this IServiceCollection services, string corsPolicy)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                corsPolicy,
                policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost",
                            "http://localhost:3000",
                            "http://localhost:5173"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
            );
        });
    }

    public static void AddSerilog(this ConfigureHostBuilder host)
    {
        LoggerConfiguration loggerConfiguration = new LoggerConfiguration();
        loggerConfiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(path: "/logs/log-.log", rollingInterval: RollingInterval.Day);

        if (AppEnv.IsProduction)
        {
            string errorLogginPassword = Environment.GetEnvironmentVariable(
                "SERILOG_EMAIL_PASSWORD"
            )!;
            string subject = "Template (Auth) Error";

            loggerConfiguration.WriteTo.Email(
                options: new EmailSinkOptions
                {
                    From = "app.errors.log@gmail.com",
                    To = ["astery227@gmail.com", "yyegor@outlook.com"],
                    Host = "smtp.gmail.com",
                    Port = 587,
                    ConnectionSecurity = SecureSocketOptions.StartTls,
                    Credentials = new NetworkCredential(
                        "app.errors.log@gmail.com",
                        errorLogginPassword
                    ),
                    Subject = new MessageTemplateTextFormatter(subject),
                    Body = new MessageTemplateTextFormatter(
                        "{Timestamp} [{Level}] {Message}{NewLine}{Exception}"
                    )
                },
                restrictedToMinimumLevel: LogEventLevel.Error
            );
        }

        Log.Logger = loggerConfiguration.CreateLogger();

        SelfLog.Enable(Console.Out);
        host.UseSerilog();
    }
}
