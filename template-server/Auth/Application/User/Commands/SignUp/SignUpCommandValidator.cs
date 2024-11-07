using Application.Common.FluentValidation;
using Domain.User.ValueObjects;
using FluentValidation;

namespace Application.User.Commands.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.Email).MustBeOk(Email.Create);
        RuleFor(x => x.Password).MustBeOk(Password.Create);
        RuleFor(x => x.DeviceId).MustBeOk(DeviceId.Create);
    }
}
