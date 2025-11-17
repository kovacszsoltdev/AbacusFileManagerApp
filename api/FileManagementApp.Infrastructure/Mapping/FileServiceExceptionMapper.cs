using Azure;
using FileManagementApp.Application.Common.Exceptions;

namespace FileManagementApp.Infrastructure.Mapping;

public static class FileServiceExceptionMapper
{

    /// <summary>
    /// Map Azure RequestFailedException to FileServiceException based on status codes and error codes documented here: <see href="https://learn.microsoft.com/en-us/rest/api/storageservices/blob-service-error-codes"/>
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static FileServiceException ToFileServiceException(this RequestFailedException ex)
    {
        if (TryMapFromErrorCode(ex.ErrorCode, out var errorCode))
        {
            return new FileServiceException(errorCode, ex.Message, ex);
        }

        errorCode = MapFromStatusCode(ex.Status);

        return new FileServiceException(errorCode, ex.Message, ex);
    }

    private static bool TryMapFromErrorCode(string? errorCode, out FileServiceErrorCode mappedCode)
    {
        mappedCode = errorCode switch
        {
            "BlobNotFound" => FileServiceErrorCode.FileNotFound,
            "BlobAlreadyExists" => FileServiceErrorCode.FileAlreadyExists,
            _ => FileServiceErrorCode.Unknown
        };
        return mappedCode != FileServiceErrorCode.Unknown;
    }

    private static FileServiceErrorCode MapFromStatusCode(int statusCode)
    {
        return statusCode switch
        {
            401 or 403 => FileServiceErrorCode.UnauthorizedAccess,
            408 or 500 or 502 or 503 or 504 => FileServiceErrorCode.NetworkError,
            _ => FileServiceErrorCode.Unknown
        };
    }
}
