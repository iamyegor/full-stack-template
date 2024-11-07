using Domain.DomainErrors;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using XResults;

namespace Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior() { }

    public ValidationBehavior(IValidator<TRequest>? validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (_validator == null)
        {
            return await next();
        }

        ValidationResult validationResult = await _validator.ValidateAsync(
            request,
            cancellationToken
        );

        if (validationResult.IsValid)
        {
            return await next();
        }

        string serializedError = validationResult.Errors.First().ErrorMessage;
        return (dynamic)Result.Fail(Error.Deserialize(serializedError));
    }
}
