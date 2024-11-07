using Application.Common.FluentValidation;
using Domain.User.ValueObjects;
using FluentValidation;

namespace Application.User.Commands.SignIn;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.DeviceId).MustBeOk(DeviceId.Create);
    }
}
