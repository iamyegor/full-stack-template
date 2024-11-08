using Domain.DomainErrors;
using Domain.Users.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Features.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.Tokens.Commands.RefreshAccessToken;

public record RefreshAccessTokenCommand(string? RefreshToken, string? DeviceId)
    : IRequest<Result<Infrastructure.Features.Auth.Tokens, Error>>;

public class RefreshAccessTokenCommandHandler
    : IRequestHandler<RefreshAccessTokenCommand, Result<Infrastructure.Features.Auth.Tokens, Error>>
{
    private readonly ApplicationContext _context;
    private readonly TokensGenerator _tokensGenerator;

    public RefreshAccessTokenCommandHandler(
        ApplicationContext context,
        TokensGenerator tokensGenerator
    )
    {
        _context = context;
        _tokensGenerator = tokensGenerator;
    }

    public async Task<Result<Infrastructure.Features.Auth.Tokens, Error>> Handle(
        RefreshAccessTokenCommand command,
        CancellationToken ct
    )
    {
        DeviceId deviceId = DeviceId.Create(command.DeviceId);

        Domain.Users.User? user = await GetUserByRefreshToken(command.RefreshToken, deviceId, ct);
        if (user == null || user.IsRefreshTokenExpired(deviceId))
        {
            return Errors.RefreshToken.IsInvalid;
        }

        Infrastructure.Features.Auth.Tokens tokens = _tokensGenerator.GenerateTokens(user);
        user.AddRefreshToken(new RefreshToken(tokens.RefreshToken, deviceId));

        await _context.SaveChangesAsync(ct);

        return tokens;
    }

    private async Task<Domain.Users.User?> GetUserByRefreshToken(
        string? refreshToken,
        DeviceId deviceId,
        CancellationToken ct
    )
    {
        Domain.Users.User? user = await _context.Users.FirstOrDefaultAsync(
            x =>
                x.RefreshTokens.Any(y =>
                    y.Value == refreshToken && y.DeviceId.Value == deviceId.Value
                ),
            ct
        );
        return user;
    }
}
