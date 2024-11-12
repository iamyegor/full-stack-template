using Domain.DomainErrors;
using Domain.Users.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Features.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.Password.Commands.ResetPassword;

public record ResetPasswordCommand(string Token, string NewPassword, string? DeviceId)
    : IRequest<Result<Infrastructure.Features.Auth.Tokens, Error>>;

public class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, Result<Infrastructure.Features.Auth.Tokens, Error>>
{
    private readonly ApplicationContext _context;
    private readonly TokensGenerator _tokensGenerator;

    public ResetPasswordCommandHandler(ApplicationContext context, TokensGenerator tokensGenerator)
    {
        _context = context;
        _tokensGenerator = tokensGenerator;
    }

    public async Task<Result<Infrastructure.Features.Auth.Tokens, Error>> Handle(
        ResetPasswordCommand command,
        CancellationToken ct
    )
    {
        DeviceId deviceId = DeviceId.Create(command.DeviceId);
        PasswordResetToken token = PasswordResetToken.Create(command.Token);
        Domain.Users.User? user = await _context.Users.FirstOrDefaultAsync(
            x => x.PasswordResetToken != null && x.PasswordResetToken.Value == token.Value,
            ct
        );
        if (user == null)
        {
            return Errors.PasswordResetToken.IsInvalid;
        }

        Domain.Users.ValueObjects.Password password = Domain.Users.ValueObjects.Password.Create(command.NewPassword);
        if (user.Password == password)
        {
            return Errors.PasswordReset.IsSameAsCurrent;
        }

        user.ResetPassword(password);
        Infrastructure.Features.Auth.Tokens tokens = _tokensGenerator.GenerateTokens(user);
        user.AddRefreshToken(new RefreshToken(tokens.RefreshToken, deviceId));

        await _context.SaveChangesAsync(ct);

        return tokens;
    }
}
