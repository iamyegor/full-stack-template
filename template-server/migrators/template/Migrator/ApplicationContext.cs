using Microsoft.EntityFrameworkCore;

namespace Migrator;

public class ApplicationContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        optionsBuilder.UseNpgsql(connectionString);
    }
}