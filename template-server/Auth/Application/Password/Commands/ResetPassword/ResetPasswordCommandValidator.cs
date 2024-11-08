using Application.Common.FluentValidation;
using Domain.Users.ValueObjects;
using FluentValidation;

namespace Application.Password.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Token).MustBeOk(PasswordResetToken.Create);
        RuleFor(x => x.NewPassword).MustBeOk(Domain.Users.ValueObjects.Password.Create);
        RuleFor(x => x.DeviceId).MustBeOk(DeviceId.Create);
    }
}
