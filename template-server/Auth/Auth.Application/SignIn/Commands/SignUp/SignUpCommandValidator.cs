using Application.Common.FluentValidation;
using Domain.Users.ValueObjects;
using FluentValidation;

namespace Application.SignIn.Commands.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.Email).MustBeOk(Domain.Users.ValueObjects.Email.Create);
        RuleFor(x => x.Password).MustBeOk(Domain.Users.ValueObjects.Password.Create);
        RuleFor(x => x.DeviceId).MustBeOk(DeviceId.Create);
    }
}
