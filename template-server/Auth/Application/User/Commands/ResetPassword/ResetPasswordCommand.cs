using Domain.DomainErrors;
using Domain.User.ValueObjects;
using Infrastructure.Auth.Authentication;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.User.Commands.ResetPassword;

public record ResetPasswordCommand(string Token, string NewPassword, string? DeviceId)
    : IRequest<Result<Tokens, Error>>;

public class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, Result<Tokens, Error>>
{
    private readonly ApplicationContext _context;
    private readonly TokensGenerator _tokensGenerator;

    public ResetPasswordCommandHandler(ApplicationContext context, TokensGenerator tokensGenerator)
    {
        _context = context;
        _tokensGenerator = tokensGenerator;
    }

    public async Task<Result<Tokens, Error>> Handle(
        ResetPasswordCommand command,
        CancellationToken ct
    )
    {
        DeviceId deviceId = DeviceId.Create(command.DeviceId);
        PasswordResetToken token = PasswordResetToken.Create(command.Token);
        Domain.User.User? user = await _context.Users.FirstOrDefaultAsync(
            x => x.PasswordResetToken != null && x.PasswordResetToken.Value == token.Value,
            ct
        );
        if (user == null)
        {
            return Errors.PasswordResetToken.IsInvalid;
        }

        Password password = Password.Create(command.NewPassword);
        if (user.Password == password)
        {
            return Errors.PasswordReset.IsSameAsCurrent;
        }

        user.ResetPassword(password);
        Tokens tokens = _tokensGenerator.GenerateTokens(user);
        user.AddRefreshToken(new RefreshToken(tokens.RefreshToken, deviceId));

        await _context.SaveChangesAsync(ct);

        return tokens;
    }
}
