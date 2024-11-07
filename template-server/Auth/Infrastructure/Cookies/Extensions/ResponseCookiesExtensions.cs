using Infrastructure.Auth.Authentication;
using Microsoft.AspNetCore.Http;
using SharedKernel.Auth;

namespace Infrastructure.Cookies.Extensions;

public static class ResponseCookiesExtensions
{
    public static void Append(this IResponseCookies cookies, Tokens tokens)
    {
        cookies.Append(
            CookiesSettings.AccessToken.Name,
            tokens.AccessToken,
            CookiesSettings.AccessToken.Options
        );
        cookies.Append(
            CookiesSettings.RefreshToken.Name,
            tokens.RefreshToken,
            CookiesSettings.RefreshToken.Options
        );
    }
}
