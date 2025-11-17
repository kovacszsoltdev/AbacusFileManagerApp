namespace FileManagementApp.Application.Common.Models;

public record FileEntry(string FileName, long? SizeInBytes, DateTimeOffset? UploadedAt);