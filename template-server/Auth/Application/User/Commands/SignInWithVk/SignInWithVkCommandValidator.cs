using Application.Common.FluentValidation;
using Domain.User.ValueObjects;
using FluentValidation;

namespace Application.User.Commands.SignInWithVk;

public class SignInWithVkCommandValidator : AbstractValidator<SignInWithVkCommand>
{
    public SignInWithVkCommandValidator()
    {
        RuleFor(x => x.DeviceId).MustBeOk(DeviceId.Create);
    }
}
