using Domain.DomainErrors;
using Domain.User.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Emails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.User.Commands.ResendEmailCode;

public record ResendEmailCodeCommand(UserId UserId) : IRequest<SuccessOr<Error>>;

public class ResendEmailCodeCommandHandler
    : IRequestHandler<ResendEmailCodeCommand, SuccessOr<Error>>
{
    private readonly ApplicationContext _context;
    private readonly DomainEmailSender _emailSender;

    public ResendEmailCodeCommandHandler(ApplicationContext context, DomainEmailSender emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }

    public async Task<SuccessOr<Error>> Handle(ResendEmailCodeCommand command, CancellationToken ct)
    {
        Domain.User.User? user = await _context.Users.SingleOrDefaultAsync(
            x => x.Id == command.UserId,
            ct
        );
        if (user == null)
        {
            return Errors.User.NotFound;
        }

        if (user.Email == null)
        {
            return Errors.Email.IsRequired;
        }

        user.NewVerificationCode();

        await _emailSender.SendEmailVerificationCode(
            user.Email.Value,
            user.EmailVerificationCode!.Value
        );

        await _context.SaveChangesAsync(ct);

        return Result.Ok();
    }
}
