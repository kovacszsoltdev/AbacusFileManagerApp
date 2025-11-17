namespace FileManagementApp.Application.Features.Files.Commands.Download;

public record DownloadFileResponse(string FileName, Stream FileStream, string ContentType);
