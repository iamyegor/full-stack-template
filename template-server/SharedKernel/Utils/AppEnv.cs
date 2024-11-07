namespace SharedKernel.Utils;

public class AppEnv
{
    public static bool IsDevelopment => CheckIsDevelopment();
    public static bool IsProduction => !CheckIsDevelopment();

    public static bool CheckIsDevelopment()
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;
        bool isDevelopment = environment == "Development";

        return isDevelopment;
    }
}
