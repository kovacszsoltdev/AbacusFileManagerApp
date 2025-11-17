namespace FileManagementApp.Application.Common.Dtos;

public static class ErrorCodes
{
	public const string UNEXPECTED = nameof(UNEXPECTED);
	public const string VALIDATION_ERROR = nameof(VALIDATION_ERROR);

    public static class Validation
	{
		public const string INVALID_FILE_NAME = nameof(INVALID_FILE_NAME);
		public const string INVALID_FILE_EXTENSION = nameof(INVALID_FILE_EXTENSION);
		public const string INVALID_CONTENT_TYPE = nameof(INVALID_CONTENT_TYPE);
		public const string FILE_STREAM_NOT_READABLE = nameof(FILE_STREAM_NOT_READABLE);
    }

	public static class FileService
	{
        public const string FILE_TOO_LARGE = nameof(FILE_TOO_LARGE);
        public const string FILE_NOT_FOUND = nameof(FILE_NOT_FOUND);
		public const string FILE_ALREADY_EXISTS = nameof(FILE_ALREADY_EXISTS);
        public const string UNAUTHORIZED_FILE_ACCESS = nameof(UNAUTHORIZED_FILE_ACCESS);
        public const string FILE_SERVICE_NOT_AVAILABLE = nameof(FILE_SERVICE_NOT_AVAILABLE);
    }
}