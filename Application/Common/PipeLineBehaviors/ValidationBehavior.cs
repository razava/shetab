using FluentValidation;
using MediatR;

namespace Application.Common.PipeLineBehaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//where TRequest : IRequest   
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context)));


        var validationFailure = validationResults.FirstOrDefault();


        if (validationFailure != null && (!validationFailure.IsValid))
        {
            throw new ValidationException(validationFailure.Errors);
        }

        var response = await next();

        return response;

    }
}


