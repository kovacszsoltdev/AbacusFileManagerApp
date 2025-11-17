using FileManagementApp.Application.Common.Dtos;
using FileManagementApp.Application.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FileManagementApp.Application.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : ApplicationResult, new()
{
    private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger;

    public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch(FileServiceException fse)
        {
            logger.LogError(fse, "File service error handling {RequestType}", typeof(TRequest).Name);
            return new TResponse
            {
                IsSuccess = false,
                Error = ApplicationError.FileServiceError(fse),
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception handling {RequestType}", typeof(TRequest).Name);
            return new TResponse
            {
                IsSuccess = false,
                Error = ApplicationError.Unexpected("An unexpected error occurred.", exception: ex)
            };
        }
    }
}