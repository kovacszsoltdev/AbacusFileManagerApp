using FileManagementApp.Application.Common.Dtos;
using FileManagementApp.Application.Common.Interfaces;
using FileManagementApp.Application.Common.Models;
using MediatR;

namespace FileManagementApp.Application.Features.Files.Commands.Upload;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, ApplicationResult>
{
    private readonly IFileService fileService;

    public UploadFileCommandHandler(IFileService fileService)
    {
        this.fileService = fileService;
    }

    public async Task<ApplicationResult> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var maxSizeInBytes = 50 * 1024 * 1024; // 50 MB limit

        await fileService.UploadAsync(new FileUploadRequest(request.FileName, request.Stream), maxSizeInBytes, cancellationToken);

        return ApplicationResult.Success();
    }
}
