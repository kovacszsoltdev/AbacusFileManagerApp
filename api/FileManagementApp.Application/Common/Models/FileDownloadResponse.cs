namespace FileManagementApp.Application.Common.Models;

public record FileDownloadResponse(string FileName, Stream Content, string ContentType);
