using Api.DiExtensions;
using Api.Utils;
using Application;
using Infrastructure;
using SharedKernel.Utils;

namespace Api;

public static class Startup
{
    private const string CorsPolicy = "myCorsPolicy";

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        if (AppEnv.IsProduction)
            builder.WebHost.UseSentry();

        builder.Host.AddSerilog();

        builder
            .Services.AddBaseServices(CorsPolicy)
            .AddApplication()
            .AddInfrastructureServices(builder.Configuration, builder.Environment.IsDevelopment())
            .AddMassTransit(builder.Configuration);

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

        app.MapControllers();

        return app;
    }
}
