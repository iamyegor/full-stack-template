using System.Reflection;
using Domain.User;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure.Data;

public class ApplicationContext : DbContext
{
    private readonly string _connectionString;
    private readonly bool _useLogger;

    public ApplicationContext() { }

    public ApplicationContext(string connectionString, bool useLogger)
    {
        _connectionString = connectionString;
        _useLogger = useLogger;
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);

        if (_useLogger)
        {
            optionsBuilder
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog()))
                .EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<T?> Query<T>(ISingleSpecification<T> spec, CancellationToken ct)
        where T : class
    {
        IQueryable<T> query = Set<T>().AsQueryable();
        query = spec.Apply(query);
        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<T>> Query<T>(
        IMultipleSpecification<T> spec,
        CancellationToken ct
    )
        where T : class
    {
        IQueryable<T> query = Set<T>().AsQueryable();
        query = spec.Apply(query);
        return await query.ToListAsync(ct);
    }
}
