using FileManagementApp.Application.Common.Dtos;
using MediatR;

namespace FileManagementApp.Application.Features.Files.Commands.Download;

public record DownloadFileQuery(string FileName): IRequest<ApplicationResult<DownloadFileResponse>>;
