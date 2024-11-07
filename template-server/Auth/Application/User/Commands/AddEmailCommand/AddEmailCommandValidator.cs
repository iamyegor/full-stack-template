using Application.Common.FluentValidation;
using Domain.User.ValueObjects;
using FluentValidation;

namespace Application.User.Commands.AddEmailCommand;

public class AddEmailCommandValidator : AbstractValidator<AddNameAndEmailCommand>
{
    public AddEmailCommandValidator()
    {
        RuleFor(x => x.Email).MustBeOk(Email.Create);
    }
}
