using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var rabbitmq = builder.AddRabbitMQ("rabbitmq");

var cache = builder.AddRedis("directus-cache");

var database = builder
    .AddPostgres("directus-db")
    .WithEnvironment("POSTGRES_DB", "directus")
    .WithEnvironment("POSTGRES_USER", "directus")
    .WithEnvironment("POSTGRES_PASSWORD", "directus");

// Add Directus container with dependencies
builder
    .AddContainer("directus", "directus/directus")
    .WithEnvironment("SECRET", "secure-state-united-state")
    // Database configuration
    .WithEnvironment("DB_CLIENT", "pg")
    .WithEnvironment("DB_HOST", "directus-db")
    .WithEnvironment("DB_PORT", "5432")
    .WithEnvironment("DB_DATABASE", "directus")
    .WithEnvironment("DB_USER", "directus")
    .WithEnvironment("DB_PASSWORD", "directus")
    // Cache configuration
    .WithEnvironment("CACHE_ENABLED", "true")
    .WithEnvironment("CACHE_AUTO_PURGE", "true")
    .WithEnvironment("CACHE_STORE", "redis")
    .WithEnvironment("REDIS", "redis://directus-cache:6379")
    // Admin credentials
    .WithEnvironment("ADMIN_EMAIL", "admin@example.com")
    .WithEnvironment("ADMIN_PASSWORD", "d1r3ctu5")
    .WithReference(cache)
    .WithReference(database);

// Console.WriteLine(await database.Resource.GetConnectionStringAsync());

var app = builder.AddProject<Api>("app").WithReference(rabbitmq);
var auth = builder.AddProject<Auth_Api>("auth").WithReference(rabbitmq);

builder.Build().Run();
