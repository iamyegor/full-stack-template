using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        ConfigurationBuilder configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonFile("appsettings.Development.json");
        IConfigurationRoot config = configBuilder.Build();

        return new ApplicationContext(config.GetConnectionString("Default")!, false);
    }
}
