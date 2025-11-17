namespace FileManagementApp.Application.Common.Models;

public record FileUploadRequest(string FileName, Stream Content);
