
namespace FileManagementApp.Application.Common.Exceptions;

public enum FileServiceErrorCode
{
    Unknown,
    FileNotFound,
    FileAlreadyExists,
    UnauthorizedAccess,
    NetworkError,
    FileTooLarge
}