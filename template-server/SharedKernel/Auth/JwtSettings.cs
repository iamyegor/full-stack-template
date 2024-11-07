namespace SharedKernel.Auth;

public class JwtSettings
{
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
}