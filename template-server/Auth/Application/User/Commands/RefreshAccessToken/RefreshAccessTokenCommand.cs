using Domain.DomainErrors;
using Domain.User.ValueObjects;
using Infrastructure.Auth.Authentication;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.User.Commands.RefreshAccessToken;

public record RefreshAccessTokenCommand(string? RefreshToken, string? DeviceId)
    : IRequest<Result<Tokens, Error>>;

public class RefreshAccessTokenCommandHandler
    : IRequestHandler<RefreshAccessTokenCommand, Result<Tokens, Error>>
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

    public async Task<Result<Tokens, Error>> Handle(
        RefreshAccessTokenCommand command,
        CancellationToken ct
    )
    {
        DeviceId deviceId = DeviceId.Create(command.DeviceId);

        Domain.User.User? user = await GetUserByRefreshToken(command.RefreshToken, deviceId, ct);
        if (user == null || user.IsRefreshTokenExpired(deviceId))
        {
            return Errors.RefreshToken.IsInvalid;
        }

        Tokens tokens = _tokensGenerator.GenerateTokens(user);
        user.AddRefreshToken(new RefreshToken(tokens.RefreshToken, deviceId));

        await _context.SaveChangesAsync(ct);

        return tokens;
    }

    private async Task<Domain.User.User?> GetUserByRefreshToken(
        string? refreshToken,
        DeviceId deviceId,
        CancellationToken ct
    )
    {
        Domain.User.User? user = await _context.Users.FirstOrDefaultAsync(
            x =>
                x.RefreshTokens.Any(y =>
                    y.Value == refreshToken && y.DeviceId.Value == deviceId.Value
                ),
            ct
        );
        return user;
    }
}
