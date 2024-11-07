using Domain.DomainErrors;
using Domain.User.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Emails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.User.Commands.AddEmailCommand;

public record AddNameAndEmailCommand(UserId UserId, string Name, string Email)
    : IRequest<SuccessOr<Error>>;

public class AddNameAndEmailCommandHandler
    : IRequestHandler<AddNameAndEmailCommand, SuccessOr<Error>>
{
    private readonly ApplicationContext _context;
    private readonly DomainEmailSender _emailSender;

    public AddNameAndEmailCommandHandler(DomainEmailSender emailSender, ApplicationContext context)
    {
        _emailSender = emailSender;
        _context = context;
    }

    public async Task<SuccessOr<Error>> Handle(AddNameAndEmailCommand command, CancellationToken ct)
    {
        Email email = Email.Create(command.Email).Value;
        Domain.User.User? userWithSameEmail = await _context.Users.SingleOrDefaultAsync(
            x => x.Email != null && x.Email.Value == email.Value,
            ct
        );
        if (userWithSameEmail != null)
        {
            if (userWithSameEmail.IsEmailVerified)
            {
                return Errors.User.EmailAlreadyExists;
            }
            else
            {
                userWithSameEmail.ResetEmail();
                await _context.SaveChangesAsync(ct);
            }
        }

        Domain.User.User? user = await _context.Users.SingleOrDefaultAsync(
            x => x.Id == command.UserId,
            ct
        );
        if (user == null || (user.Email != null && user.IsEmailVerified))
        {
            return Errors.User.AddEmailError;
        }

        user.AddEmail(email);

        await _context.SaveChangesAsync(ct);

        await _emailSender.SendEmailVerificationCode(
            user.Email!.Value,
            user.EmailVerificationCode!.Value
        );

        return Result.Ok();
    }
}
