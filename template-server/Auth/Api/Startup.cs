using System.Reflection;
using Api.DiExtensions;
using Api.Middlewares;
using Application;
using Infrastructure;
using SharedKernel.Communication.Extensions;

namespace Api;

public static class Startup
{
    private const string CorsPolicy = "myCorsPolicy";

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.WebHost.UseSentry();
        builder.Host.AddSerilog();

        builder
            .Services.AddApiServices(CorsPolicy)
            .AddInfrastructureServices(builder.Configuration, builder.Environment.IsDevelopment())
            .AddApplicationValidation()
            .AddMassTransit(builder.Configuration, Assembly.GetExecutingAssembly(), "Auth");

        return builder.Build();
    }

    public static WebApplication ConfigureMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(CorsPolicy);
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseRateLimiter();

        app.MapControllers();

        return app;
    }
}
