using Application.Common.FluentValidation;
using Domain.User.ValueObjects;
using FluentValidation;

namespace Application.User.Commands.ConfirmEmail;

public class VerifyEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(x => x.Code).MustBeOk(EmailVerificationCode.Create);
    }
}
