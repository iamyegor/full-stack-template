using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.Data;

public class DatabaseAvailabilityChecker
{
    public static async Task WaitForDatabaseAsync(IServiceProvider serviceProvider)
    {
        Log.Information("Trying to connect to the database...");

        const int retryCount = 3;
        const int waitTimeSeconds = 5;

        using IServiceScope scope = serviceProvider.CreateScope();
        ApplicationContext context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

        for (int i = 0; i < retryCount; i++)
        {
            try
            {
                await context.Database.CanConnectAsync();
                Log.Information("Successfully connected to the database.");
                return;
            }
            catch (Exception ex)
            {
                Log.Information(
                    $"Database connection attempt {i + 1} failed. Retrying in {waitTimeSeconds} seconds..."
                );
                Log.Error($"Error: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(waitTimeSeconds));
            }
        }

        throw new Exception($"Unable to connect to the database after {retryCount} attempts.");
    }
}
