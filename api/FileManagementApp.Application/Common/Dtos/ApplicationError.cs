using FileManagementApp.Application.Common.Exceptions;

namespace FileManagementApp.Application.Common.Dtos;

public class ApplicationError
{
    public string Message { get; init; }
    public string Code { get; init; }
    public Dictionary<string, object> Details { get; init; }
    public Exception? Exception { get; init; }

    private ApplicationError(string message, string code, Dictionary<string, object>? details = null, Exception? exception = null)
    {
        Message = message;
        Code = code;
        Details = details ?? new Dictionary<string, object>();
        Exception = exception;
    }

    public static ApplicationError Default(string message, string errorCode, Dictionary<string, object>? details = null, Exception? exception = null)
    {
        return new ApplicationError(message, errorCode, details, exception);
    }

    public static ApplicationError Unexpected(string message, Dictionary<string, object>? details = null, Exception? exception = null)
    {
        return new ApplicationError(message, ErrorCodes.UNEXPECTED, details, exception);
    }

    public static ApplicationError Validation(string message, Dictionary<string, string[]> errors)
    {
        return new ApplicationError(
            message, 
            ErrorCodes.VALIDATION_ERROR,
            new Dictionary<string, object> { { "validation", errors } });
    }

    public static ApplicationError FileServiceError(FileServiceException fileServiceException)
    {
        var code = fileServiceException.ErrorCode switch
        {
            FileServiceErrorCode.FileNotFound => ErrorCodes.FileService.FILE_NOT_FOUND,
            FileServiceErrorCode.FileAlreadyExists => ErrorCodes.FileService.FILE_ALREADY_EXISTS,
            FileServiceErrorCode.FileTooLarge => ErrorCodes.FileService.FILE_TOO_LARGE,
            FileServiceErrorCode.UnauthorizedAccess => ErrorCodes.FileService.UNAUTHORIZED_FILE_ACCESS,
            FileServiceErrorCode.NetworkError => ErrorCodes.FileService.FILE_SERVICE_NOT_AVAILABLE,            
            _ => ErrorCodes.UNEXPECTED
        };

        return new ApplicationError(
            "A file service error occurred.",
            code,
            exception: fileServiceException);
    }
}
