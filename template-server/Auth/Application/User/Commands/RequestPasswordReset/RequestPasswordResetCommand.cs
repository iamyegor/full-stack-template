using Domain.DomainErrors;
using Infrastructure.Data;
using Infrastructure.Emails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.User.Commands.RequestPasswordReset;

public record RequestPasswordResetCommand(string Email) : IRequest<SuccessOr<Error>>;

public class RequestPasswordResetCommandHandler
    : IRequestHandler<RequestPasswordResetCommand, SuccessOr<Error>>
{
    private readonly ApplicationContext _context;
    private readonly DomainEmailSender _emailSender;

    public RequestPasswordResetCommandHandler(
        ApplicationContext context,
        DomainEmailSender passwordResetCodeSender
    )
    {
        _context = context;
        _emailSender = passwordResetCodeSender;
    }

    public async Task<SuccessOr<Error>> Handle(
        RequestPasswordResetCommand command,
        CancellationToken ct
    )
    {
        Domain.User.User? user = await _context.Users.SingleOrDefaultAsync(
            x => x.Email != null && x.Email.Value == command.Email,
            ct
        );
        if (user == null || !user.IsEmailVerified)
        {
            return Errors.User.NotFound;
        }

        if (user.PasswordResetToken != null && !user.PasswordResetToken.IsExpired)
        {
            return Errors.PasswordReset.IsAlreadyRequested;
        }

        user.GeneratePasswordResetToken();
        await _context.SaveChangesAsync(ct);

        await _emailSender.SendPasswordReset(user.PasswordResetToken!, user.Email!.Value);

        return Result.Ok();
    }
}
