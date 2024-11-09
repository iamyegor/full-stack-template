using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Helpers;

public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        string currentDir = Directory.GetCurrentDirectory();
        string configPath = Path.GetFullPath(
            Path.Combine(currentDir, "..", "Api")
        );

        ConfigurationBuilder configBuilder = new ConfigurationBuilder();
        configBuilder.SetBasePath(configPath).AddJsonFile("appsettings.Development.json");

        IConfigurationRoot config = configBuilder.Build();

        return new ApplicationContext(config.GetConnectionString("Default")!, false);
    }
}
