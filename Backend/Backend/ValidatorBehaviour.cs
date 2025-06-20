using FluentValidation;
using MediatR;
using Backend.BusinessLogic.Exception;
using System.Net;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;

  public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
  {
    _validators = validators;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    if (_validators.Any())
    {
      var context = new ValidationContext<TRequest>(request);

      var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
      var failures = validationResults
          .SelectMany(result => result.Errors)
          .Where(error => error != null)
          .ToList();

      if (failures.Count > 0)
      {
        var errorMessage = string.Join(" | ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));

        throw new AbstractValidatorException(HttpStatusCode.BadRequest, errorMessage);
      }
    }

    return await next();
  }
}
