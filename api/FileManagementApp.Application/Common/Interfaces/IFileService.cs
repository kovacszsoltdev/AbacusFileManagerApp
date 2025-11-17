using FileManagementApp.Application.Common.Models;

namespace FileManagementApp.Application.Common.Interfaces;

public interface IFileService
{
    Task UploadAsync(FileUploadRequest request, long maxSizeInBytes, CancellationToken cancellationToken = default);

    Task<FileDownloadResponse> DownloadAsync(string fileName, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FileEntry>> GetAllAsync(CancellationToken cancellationToken = default);

    Task DeleteAsync(string fileName, CancellationToken cancellationToken = default);
}
