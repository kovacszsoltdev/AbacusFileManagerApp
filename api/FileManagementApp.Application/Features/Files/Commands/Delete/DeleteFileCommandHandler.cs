using FileManagementApp.Application.Common.Dtos;
using FileManagementApp.Application.Common.Interfaces;
using MediatR;

namespace FileManagementApp.Application.Features.Files.Commands.Delete;

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, ApplicationResult>
{
    private readonly IFileService fileService;

    public DeleteFileCommandHandler(IFileService fileService)
    {
        this.fileService = fileService;
    }

    public async Task<ApplicationResult> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        await fileService.DeleteAsync(request.FileName, cancellationToken);
        return ApplicationResult.Success();
    }
}
