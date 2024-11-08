using Application.Common.FluentValidation;
using Domain.Users.ValueObjects;
using FluentValidation;

namespace Application.Email.Commands.ConfirmEmail;

public class VerifyEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(x => x.Code).MustBeOk(EmailVerificationCode.Create);
    }
}
