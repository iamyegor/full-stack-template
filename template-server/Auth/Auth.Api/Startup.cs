using Api.DiExtensions;
using Api.Middlewares;
using Application;
using Infrastructure;

namespace Api;

public static class Startup
{
    private const string CorsPolicy = "myCorsPolicy";

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        
        builder.WebHost.UseSentry();
        builder.Host.AddSerilog();

        builder
            .Services.AddApiServices(CorsPolicy)
            .AddInfrastructureServices(builder.Configuration, builder.Environment.IsDevelopment())
            .AddApplicationValidation();

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
