using Application.Common.FluentValidation;
using Domain.Users.ValueObjects;
using FluentValidation;

namespace Application.Tokens.Commands.RefreshAccessToken;

public class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
        RuleFor(x => x.DeviceId).MustBeOk(DeviceId.Create);
    }
}
