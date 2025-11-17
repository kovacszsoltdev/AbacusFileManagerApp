using FileManagementApp.Application.Common.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FileManagementApp.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : ApplicationResult, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> logger;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        this.logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var result = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = result.SelectMany(r => r.Errors)
                                 .ToList();

            if (failures.Count != 0)
            {
                logger.LogWarning("Validation errors - {RequestType} - Errors: {@ValidationErrors}", typeof(TRequest).Name, failures);

                return new TResponse
                {
                    IsSuccess = false,
                    Error = ApplicationError.Validation("One or more validation errors occurred.",
                        failures.GroupBy(f => f.PropertyName)
                                .ToDictionary(
                                    g => g.Key, 
                                    g => g.Select(f => f.ErrorCode).ToArray()
                                ))
                };
            }
        }

        return await next();
    }
}
