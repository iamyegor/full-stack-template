using Application.Common.FluentValidation;
using Domain.Users.ValueObjects;
using FluentValidation;

namespace Application.SignIn.Commands.SignInWithVk;

public class SignInWithVkCommandValidator : AbstractValidator<SignInWithVkCommand>
{
    public SignInWithVkCommandValidator()
    {
        RuleFor(x => x.DeviceId).MustBeOk(DeviceId.Create);
    }
}
