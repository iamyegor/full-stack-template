using Domain.DomainErrors;
using Domain.Users.ValueObjects;
using Google.Apis.Auth;
using Infrastructure.Data;
using Infrastructure.Features.Auth;
using Infrastructure.Features.Auth.Google;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.SignIn.Commands.SignInWithGoogle;

public record SignInWithGoogleCommand(string IdToken, string? DeviceId)
    : IRequest<Result<Infrastructure.Features.Auth.Tokens, Error>>;

public class SignInWithGoogleCommandHandler
    : IRequestHandler<SignInWithGoogleCommand, Result<Infrastructure.Features.Auth.Tokens, Error>>
{
    private readonly ApplicationContext _context;
    private readonly TokensGenerator _tokensGenerator;
    private readonly GoogleIdTokenValidator _googleIdTokenValidator;

    public SignInWithGoogleCommandHandler(
        TokensGenerator tokensGenerator,
        ApplicationContext context,
        GoogleIdTokenValidator googleIdTokenValidator
    )
    {
        _tokensGenerator = tokensGenerator;
        _context = context;
        _googleIdTokenValidator = googleIdTokenValidator;
    }

    public async Task<Result<Infrastructure.Features.Auth.Tokens, Error>> Handle(
        SignInWithGoogleCommand command,
        CancellationToken ct
    )
    {
        DeviceId deviceId = DeviceId.Create(command.DeviceId);

        Result<GoogleJsonWebSignature.Payload, Error> payloadOrError =
            await _googleIdTokenValidator.ValidateAsync(command.IdToken);

        if (payloadOrError.IsFailure)
            return payloadOrError.Error;

        Domain.Users.ValueObjects.Email email = Domain.Users.ValueObjects.Email.Create(payloadOrError.Value.Email).Value;
        Domain.Users.User? user = await _context.Users.FirstOrDefaultAsync(
            x => x.Email == email,
            ct
        );

        if (user?.IsEmailVerified == false)
            user.ConfirmEmail();

        if (user == null)
            user = Domain.Users.User.CreateOAuthUser(email);

        Infrastructure.Features.Auth.Tokens tokens = _tokensGenerator.GenerateTokens(user);
        user.AddRefreshToken(new RefreshToken(tokens.RefreshToken, deviceId));

        await _context.SaveChangesAsync(ct);

        return Result.Ok(tokens);
    }
}
