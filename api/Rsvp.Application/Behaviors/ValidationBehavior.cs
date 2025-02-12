namespace Rsvp.Application.Behaviors;

using Ardalis.Result;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.Logging;

public class ValidationBehavior<TRequest, TResponse>(
  IEnumerable<IValidator<TRequest>> validators,
  ILogger<ValidationBehavior<TRequest, TResponse>> logger)
  : IPipelineBehavior<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken cancellationToken)
  {
    if (!validators.Any())
    {
      return await next();
    }

    var context = new ValidationContext<TRequest>(request);
    var validationFailures = validators
      .Select(v => v.Validate(context))
      .SelectMany(result => result.Errors)
      .Where(f => f != null)
      .ToList();

    if (validationFailures.Count == 0)
    {
      return await next();
    }

    foreach (var failure in validationFailures)
    {
      logger.LogWarning("Validation Error: Property {PropertyName}, Error: {ErrorMessage}",
        failure.PropertyName, failure.ErrorMessage);
    }

    // Return a typed Result<TResponse>
    var errors = validationFailures.Select(f => f.ErrorMessage).ToList();
    var validationErrors = errors.Select(s => new ValidationError(s)).ToList();

    // Use reflection to return Result<TResponse>.Invalid() dynamically
    var invalidResult = typeof(Result<>)
      .MakeGenericType(typeof(TResponse).GetGenericArguments()[0])
      .GetMethod(nameof(Result<object>.Invalid), [typeof(IEnumerable<ValidationError>)])
      ?.Invoke(null, [validationErrors]);

    return (TResponse)invalidResult!;
  }
}
