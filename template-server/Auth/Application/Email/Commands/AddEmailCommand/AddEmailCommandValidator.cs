using Application.Common.FluentValidation;
using FluentValidation;

namespace Application.Email.Commands.AddEmailCommand;

public class AddEmailCommandValidator : AbstractValidator<AddNameAndEmailCommand>
{
    public AddEmailCommandValidator()
    {
        RuleFor(x => x.Email).MustBeOk(Domain.Users.ValueObjects.Email.Create);
    }
}
