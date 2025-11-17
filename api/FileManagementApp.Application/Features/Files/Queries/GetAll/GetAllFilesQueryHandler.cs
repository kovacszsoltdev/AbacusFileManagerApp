using FileManagementApp.Application.Common.Dtos;
using FileManagementApp.Application.Common.Interfaces;
using MediatR;

namespace FileManagementApp.Application.Features.Files.Commands.GetAll;

public class GetAllFilesQueryHandler : IRequestHandler<GetAllFilesQuery, ApplicationResult<IReadOnlyList<FileResponse>>>
{
    private readonly IFileService fileService;

    public GetAllFilesQueryHandler(IFileService fileService)
    {
        this.fileService = fileService;
    }

    public async Task<ApplicationResult<IReadOnlyList<FileResponse>>> Handle(GetAllFilesQuery request, CancellationToken cancellationToken)
    {
        var files = await fileService.GetAllAsync(cancellationToken);

        var fileEntries = files.Select(fe => new FileResponse(fe.FileName, fe.SizeInBytes, fe.UploadedAt)).ToList();
        return ApplicationResult.Success((IReadOnlyList<FileResponse>)fileEntries);
    }
}
