using Domain.DomainErrors;
using Domain.User.ValueObjects;
using Infrastructure.Data;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Communication.Events;
using XResults;

namespace Application.User.Commands.ConfirmEmail;

public record ConfirmEmailCommand(UserId UserId, string Code) : IRequest<SuccessOr<Error>>;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, SuccessOr<Error>>
{
    private readonly ApplicationContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public ConfirmEmailCommandHandler(ApplicationContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<SuccessOr<Error>> Handle(ConfirmEmailCommand request, CancellationToken ct)
    {
        EmailVerificationCode code = EmailVerificationCode.Create(request.Code);
        Domain.User.User? user = await _context.Users.SingleOrDefaultAsync(
            x => x.Id == request.UserId,
            ct
        );
        if (
            user == null
            || user.EmailVerificationCode == null
            || user.EmailVerificationCode != code
        )
        {
            return Errors.EmailVerificationCode.IsInvalid;
        }

        if (user.EmailVerificationCode.IsExpired)
        {
            return Errors.EmailVerificationCode.IsExpired;
        }
        
        user.ConfirmEmail();
        await _context.SaveChangesAsync(ct);

        await _publishEndpoint.Publish(
            new UserRegisteredEvent(user.Id.Value, user.Email!.Value),
            ct
        );

        return Result.Ok();
    }
}
