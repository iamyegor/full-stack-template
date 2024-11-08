using Domain.DomainErrors;
using Domain.Users.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Features.Auth;
using Infrastructure.Features.Emails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.SignIn.Commands.SignUp;

public record SignUpCommand(string Email, string Password, string? DeviceId)
    : IRequest<Result<Infrastructure.Features.Auth.Tokens, Error>>;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Result<Infrastructure.Features.Auth.Tokens, Error>>
{
    private readonly ApplicationContext _context;
    private readonly DomainEmailSender _emailSender;
    private readonly TokensGenerator _tokensGenerator;
    private readonly UserRemover _userRemover;

    public SignUpCommandHandler(
        ApplicationContext context,
        TokensGenerator tokensGenerator,
        DomainEmailSender emailSender,
        UserRemover userRemover
    )
    {
        _context = context;
        _tokensGenerator = tokensGenerator;
        _emailSender = emailSender;
        _userRemover = userRemover;
    }

    public async Task<Result<Infrastructure.Features.Auth.Tokens, Error>> Handle(SignUpCommand command, CancellationToken ct)
    {
        Domain.Users.ValueObjects.Password password = Domain.Users.ValueObjects.Password.Create(command.Password);
        Domain.Users.ValueObjects.Email email = Domain.Users.ValueObjects.Email.Create(command.Email);
        DeviceId deviceId = DeviceId.Create(command.DeviceId);

        Error? userAlreadyExistsError = await GetErrorIfUserAlreadyExists(email, ct);
        if (userAlreadyExistsError != null)
        {
            return userAlreadyExistsError;
        }

        Domain.Users.User user = new Domain.Users.User(email, password);

        Infrastructure.Features.Auth.Tokens tokens = _tokensGenerator.GenerateTokens(user);
        user.AddRefreshToken(new RefreshToken(tokens.RefreshToken, deviceId));

        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);

        await _emailSender.SendEmailVerificationCode(
            email.Value,
            user.EmailVerificationCode!.Value
        );

        return tokens;
    }

    private async Task<Error?> GetErrorIfUserAlreadyExists(Domain.Users.ValueObjects.Email email, CancellationToken ct)
    {
        Domain.Users.User? userWithSameEmail = await _context.Users.SingleOrDefaultAsync(
            x => x.Email != null && x.Email.Value == email.Value,
            ct
        );
        if (userWithSameEmail != null && userWithSameEmail.IsEmailVerified)
        {
            return Errors.Email.IsAlreadyTaken;
        }

        await _userRemover.RemoveUserIfExists(userWithSameEmail, ct);

        return null;
    }
}
