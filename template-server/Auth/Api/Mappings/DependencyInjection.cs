using System.Reflection;
using Mapster;

namespace Api.Mappings;

public static class DependencyInjection
{
    public static void RegisterMappings(this IServiceCollection services)
    {
        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
    }
}
