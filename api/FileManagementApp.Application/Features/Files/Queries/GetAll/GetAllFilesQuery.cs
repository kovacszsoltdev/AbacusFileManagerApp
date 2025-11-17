using FileManagementApp.Application.Common.Dtos;
using MediatR;

namespace FileManagementApp.Application.Features.Files.Commands.GetAll;

public record GetAllFilesQuery(): IRequest<ApplicationResult<IReadOnlyList<FileResponse>>>;
