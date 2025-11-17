
namespace FileManagementApp.Application.Common.Exceptions;

/// <summary>
/// Exception representing errors from the file service.
/// </summary>
public class FileServiceException: Exception
{
    /// <summary>
    /// The specific error code associated with the file service error.
    /// </summary>
    public FileServiceErrorCode ErrorCode { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileServiceException"/> class with the specified error code.
    /// </summary>
    /// <param name="errorCode"></param>
    public FileServiceException(FileServiceErrorCode errorCode) : base()
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileServiceException"/> class with the specified error code and message.
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="message"></param>
    public FileServiceException(FileServiceErrorCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileServiceException"/> class with the specified error code, message, and inner exception.
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public FileServiceException(FileServiceErrorCode errorCode, string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
