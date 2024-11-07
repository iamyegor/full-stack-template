using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Auth;

namespace Infrastructure.Auth.Authentication;

public class TokensGenerator
{
    private readonly JwtSettings _jwtSettings;

    public TokensGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public Tokens GenerateTokens(User user)
    {
        Claim[] claims =
        [
            new Claim(JwtClaims.UserId, user.Id.Value.ToString()),
            new Claim(JwtClaims.IsEmailVerified, user.IsEmailVerified.ToString().ToLower())
        ];

        string accessToken = GenerateAccessToken(claims);
        string refreshToken = GenerateRefreshToken();

        return new Tokens(accessToken, refreshToken);
    }

    private string GenerateAccessToken(Claim[] claims)
    {
        string secret = Environment.GetEnvironmentVariable("SECRET")!;
        SigningCredentials signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            SecurityAlgorithms.HmacSha256
        );

        JwtSecurityToken jwt = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes(2),

            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private string GenerateRefreshToken()
    {
        byte[] randomNumbers = new byte[32];

        using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumbers);

        return Convert.ToBase64String(randomNumbers);
    }
}
