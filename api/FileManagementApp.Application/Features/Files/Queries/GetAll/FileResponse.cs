namespace FileManagementApp.Application.Features.Files.Commands.GetAll;

public record FileResponse(string FileName, long? SizeInBytes, DateTimeOffset? UploadedAt);