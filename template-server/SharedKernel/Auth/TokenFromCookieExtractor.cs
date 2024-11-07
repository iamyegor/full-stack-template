using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SharedKernel.Auth;

public class TokenFromCookieExtractor
{
    public static JwtBearerEvents Extract()
    {
        return new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                bool containsCookie = context.Request.Cookies.TryGetValue(
                    CookiesSettings.AccessToken.Name,
                    out string? accesToken
                );

                if (containsCookie)
                {
                    context.Token = accesToken;
                }

                return Task.CompletedTask;
            }
        };
    }
}