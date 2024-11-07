using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SharedKernel.Auth;

public class JwtValidationParameters
{
    private readonly JwtSettings _jwtSettings;

    public JwtValidationParameters(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public TokenValidationParameters GetParameters()
    {
        string secret = Environment.GetEnvironmentVariable("SECRET")!;

        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        };
    }
}