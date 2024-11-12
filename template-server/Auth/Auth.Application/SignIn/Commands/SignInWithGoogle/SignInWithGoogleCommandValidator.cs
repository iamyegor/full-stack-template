using Application.Common.FluentValidation;
using Domain.Users.ValueObjects;
using FluentValidation;

namespace Application.SignIn.Commands.SignInWithGoogle;

public class SignInWithGoogleCommandValidator : AbstractValidator<SignInWithGoogleCommand>
{
    public SignInWithGoogleCommandValidator()
    {
        RuleFor(x => x.DeviceId).MustBeOk(DeviceId.Create);
    }
}
