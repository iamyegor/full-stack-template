using Domain.DomainErrors;
using Domain.User.ValueObjects;
using Infrastructure.Auth.Authentication;
using Infrastructure.Auth.VkAuth;
using Infrastructure.Data;
using Infrastructure.Emails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.User.Commands.SignUp;

public record SignUpCommand(string Email, string Password, string? DeviceId)
    : IRequest<Result<Tokens, Error>>;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Result<Tokens, Error>>
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

    public async Task<Result<Tokens, Error>> Handle(SignUpCommand command, CancellationToken ct)
    {
        Password password = Password.Create(command.Password);
        Email email = Email.Create(command.Email);
        DeviceId deviceId = DeviceId.Create(command.DeviceId);

        Error? userAlreadyExistsError = await GetErrorIfUserAlreadyExists(email, ct);
        if (userAlreadyExistsError != null)
        {
            return userAlreadyExistsError;
        }

        Domain.User.User user = new Domain.User.User(email, password);

        Tokens tokens = _tokensGenerator.GenerateTokens(user);
        user.AddRefreshToken(new RefreshToken(tokens.RefreshToken, deviceId));

        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);

        await _emailSender.SendEmailVerificationCode(
            email.Value,
            user.EmailVerificationCode!.Value
        );

        return tokens;
    }

    private async Task<Error?> GetErrorIfUserAlreadyExists(Email email, CancellationToken ct)
    {
        Domain.User.User? userWithSameEmail = await _context.Users.SingleOrDefaultAsync(
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
