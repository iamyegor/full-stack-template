using Microsoft.Extensions.Configuration;
using SharedKernel.Utils;

namespace Infrastructure.Utils;

public class ConnectionStringResolver
{
    private readonly IConfiguration _configuration;

    public ConnectionStringResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetBasedOnEnvironment()
    {
        return AppEnv.IsDevelopment
            ? _configuration.GetConnectionString("Default")!
            : Environment.GetEnvironmentVariable("CONNECTION_STRING")!;
    }
}
