using FileManagementApp.Application.Common.Dtos;
using FileManagementApp.Application.Common.Interfaces;
using MediatR;

namespace FileManagementApp.Application.Features.Files.Commands.Download;

public class DownloadFileQueryHandler : IRequestHandler<DownloadFileQuery, ApplicationResult<DownloadFileResponse>>
{
    private readonly IFileService fileService;

    public DownloadFileQueryHandler(IFileService fileService)
    {
        this.fileService = fileService;
    }

    public async Task<ApplicationResult<DownloadFileResponse>> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
    {
        var file =  await fileService.DownloadAsync(request.FileName, cancellationToken);
        return ApplicationResult.Success(new DownloadFileResponse(file.FileName, file.Content, file.ContentType));
    }
}
