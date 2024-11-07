using Application.Common.FluentValidation;
using Domain.User.ValueObjects;
using FluentValidation;

namespace Application.User.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Token).MustBeOk(PasswordResetToken.Create);
        RuleFor(x => x.NewPassword).MustBeOk(Password.Create);
        RuleFor(x => x.DeviceId).MustBeOk(DeviceId.Create);
    }
}
