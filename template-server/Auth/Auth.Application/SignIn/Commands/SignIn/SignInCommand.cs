using Domain.DomainErrors;
using Domain.Users.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Features.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.SignIn.Commands.SignIn;

public record SignInCommand(string Email, string Password, string? DeviceId)
    : IRequest<Result<Infrastructure.Features.Auth.Tokens, Error>>;

public class SignInCommandHandler : IRequestHandler<SignInCommand, Result<Infrastructure.Features.Auth.Tokens, Error>>
{
    private readonly ApplicationContext _context;
    private readonly TokensGenerator _tokensGenerator;

    public SignInCommandHandler(ApplicationContext context, TokensGenerator tokensGenerator)
    {
        _context = context;
        _tokensGenerator = tokensGenerator;
    }

    public async Task<Result<Infrastructure.Features.Auth.Tokens, Error>> Handle(SignInCommand command, CancellationToken ct)
    {
        DeviceId deviceId = DeviceId.Create(command.DeviceId);

        Domain.Users.User? user = await _context.Users.SingleOrDefaultAsync(
            x => x.Email != null && x.Email.Value == command.Email && x.IsEmailVerified,
            ct
        );
        if (user == null || user.Password == null || !user.Password.Matches(command.Password))
        {
            return Errors.User.InvalidCredentials;
        }

        Infrastructure.Features.Auth.Tokens tokens = _tokensGenerator.GenerateTokens(user);
        user.AddRefreshToken(new RefreshToken(tokens.RefreshToken, deviceId));

        await _context.SaveChangesAsync(ct);

        return tokens;
    }
}
