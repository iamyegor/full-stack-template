using Serilog;
using Serilog.Debugging;
using Serilog.Events;

namespace Api.DiExtensions;

public static class SerilogExtensions
{
    public static void AddSerilog(this ConfigureHostBuilder host)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(path: "/logs/log-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        SelfLog.Enable(Console.Out);
        host.UseSerilog();
    }
}
