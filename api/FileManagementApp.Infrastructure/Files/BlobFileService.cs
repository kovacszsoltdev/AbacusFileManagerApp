using Azure;
using Azure.Storage.Blobs;
using FileManagementApp.Application.Common.Exceptions;
using FileManagementApp.Application.Common.Interfaces;
using FileManagementApp.Application.Common.Models;
using FileManagementApp.Infrastructure.Mapping;
using FileManagementApp.Infrastructure.Options;
using FileManagementApp.Infrastructure.Streams;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileManagementApp.Infrastructure.Files;

public class BlobFileService : IFileService
{
    private readonly BlobContainerClient _container;
    private readonly ILogger<BlobFileService> logger;

    public BlobFileService(BlobServiceClient blobServiceClient, IOptions<BlobStorageOptions> options, ILogger<BlobFileService> logger)
    {
        _container = blobServiceClient.GetBlobContainerClient(options.Value.ContainerName);
        this.logger = logger;
    }

    public async Task UploadAsync(FileUploadRequest request, long maxSizeInBytes, CancellationToken cancellationToken = default)
    {
        try
        {
            var blob = _container.GetBlobClient(request.FileName);

            var maxSizeStream = new MaxSizeStream(request.Content, maxSizeInBytes);

            await blob.UploadAsync(maxSizeStream, overwrite: false, cancellationToken);
        }
        catch (RequestFailedException ex)
        {
            logger.LogError(ex, "Error uploading file {FileName} to blob storage.", request.FileName);
            throw ex.ToFileServiceException();
        }
        catch(FileServiceException fse)
        {
            logger.LogWarning(fse, fse.Message);
            throw;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Unexpected error uploading file {FileName} to blob storage.", request.FileName);
            throw new FileServiceException(FileServiceErrorCode.Unknown, "An unexpected error occurred during file upload.", ex);
        }
    }

    public async Task DeleteAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blob = _container.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }
        catch (RequestFailedException ex)
        {
            logger.LogError(ex, "Error deleting file {FileName} from blob storage.", fileName);
            throw ex.ToFileServiceException();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting file {FileName} from blob storage.", fileName);
            throw new FileServiceException(FileServiceErrorCode.Unknown, "An unexpected error occurred during file deletion.", ex);
        }
    }

    public async Task<FileDownloadResponse> DownloadAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blob = _container.GetBlobClient(fileName);
            var response = await blob.DownloadStreamingAsync(cancellationToken: cancellationToken);            
            return new FileDownloadResponse(fileName, response.Value.Content, response.Value.Details.ContentType);
        }
        catch (RequestFailedException ex)
        {
            logger.LogError(ex, "Error downloading file {FileName} from blob storage.", fileName);
            throw ex.ToFileServiceException();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error downloading file {FileName} from blob storage.", fileName);
            throw new FileServiceException(FileServiceErrorCode.Unknown, "An unexpected error occurred during file download.", ex);
        }
    }

    public async Task<IReadOnlyList<FileEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var items = new List<FileEntry>();

            await foreach (var blob in _container.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                items.Add(new FileEntry(blob.Name, blob.Properties.ContentLength, blob.Properties.LastModified));
            }

            return items;
        }
        catch (RequestFailedException ex)
        {
            logger.LogError(ex, "Error retrieving file list from blob storage.");
            throw ex.ToFileServiceException();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error retrieving file list from blob storage.");
            throw new FileServiceException(FileServiceErrorCode.Unknown, "An unexpected error occurred during retrieving file list.", ex);
        }
    }

}
