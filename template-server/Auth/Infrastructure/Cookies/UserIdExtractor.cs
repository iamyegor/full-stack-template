using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.DomainErrors;
using Domain.User.ValueObjects;
using Infrastructure.Auth.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Auth;
using XResults;

namespace Infrastructure.Cookies;

public class UserIdExtractor
{
    private readonly JwtValidationParameters _jwtValidationParameters;

    public UserIdExtractor(JwtValidationParameters jwtValidationParameters)
    {
        _jwtValidationParameters = jwtValidationParameters;
    }

    public Result<UserId, Error> ExtractUserId(IRequestCookieCollection cookies)
    {
        cookies.TryGetValue(CookiesSettings.AccessToken.Name, out string? accessToken);

        try
        {
            JwtSecurityTokenHandler tokenSecurityHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = tokenSecurityHandler.ValidateToken(
                accessToken,
                _jwtValidationParameters.GetParameters(),
                out _
            );

            Claim? userIdClaim = principal.FindFirst(JwtClaims.UserId);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out _))
            {
                return Result.Fail(Errors.AccessToken.IsInvalid);
            }

            return new UserId(Guid.Parse(userIdClaim.Value));
        }
        catch (SecurityTokenValidationException)
        {
            return Result.Fail(Errors.AccessToken.IsInvalid);
        }
    }
}
