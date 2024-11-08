using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;
using Serilog;

namespace Infrastructure.Data.Helpers;

public static class DbAvailabilityChecker
{
    public static async Task WaitForDatabaseAsync(IServiceProvider serviceProvider)
    {
        Log.Information("Trying to connect to the database...");

        using IServiceScope scope = serviceProvider.CreateScope();

        ResiliencePipelineProvider<string> pipelineProvider =
            scope.ServiceProvider.GetRequiredService<ResiliencePipelineProvider<string>>();
        ApplicationContext context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

        ResiliencePipeline pipeline = pipelineProvider.GetPipeline("db");
        bool success = await pipeline.ExecuteAsync(async ct =>
            await context.Database.CanConnectAsync(ct)
        );

        if (success)
            Log.Information("Successfully connected to the database");
        else
            throw new Exception($"Unable to connect to the database after retrying for 3 times.");
    }
}
