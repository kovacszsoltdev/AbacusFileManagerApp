using FileManagementApp.Application.Common.Dtos;
using MediatR;

namespace FileManagementApp.Application.Features.Files.Commands.Delete;

public record DeleteFileCommand(string FileName): IRequest<ApplicationResult>;
