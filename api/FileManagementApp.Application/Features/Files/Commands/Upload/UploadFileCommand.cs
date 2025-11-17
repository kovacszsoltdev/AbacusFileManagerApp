using FileManagementApp.Application.Common.Dtos;
using MediatR;

namespace FileManagementApp.Application.Features.Files.Commands.Upload;

public record UploadFileCommand(string FileName, Stream Stream, string ContentType): IRequest<ApplicationResult>;
