using Microsoft.AspNetCore.Http;
using SharedKernel.Utils;

namespace SharedKernel.Auth;

public static class CookiesSettings
{
    public static class AccessToken
    {
        public static string Name => "accessToken";

        public static CookieOptions Options =>
            new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(30),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Domain = AppEnv.IsProduction
                    ? Environment.GetEnvironmentVariable("DOMAIN")
                    : "localhost"
            };
    }

    public static class RefreshToken
    {
        public static string Name => "refreshToken";

        public static CookieOptions Options =>
            new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(30),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Domain = AppEnv.IsProduction
                    ? Environment.GetEnvironmentVariable("DOMAIN")
                    : "localhost"
            };
    }

    public static class DeviceId
    {
        public static string Name => "deviceId";

        public static CookieOptions Options =>
            new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(365),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Domain = AppEnv.IsProduction
                    ? Environment.GetEnvironmentVariable("DOMAIN")
                    : "localhost"
            };
    }
}
