namespace FileManagementApp.Infrastructure.Options;

/// <summary>
/// Options for Blob Storage configuration.
/// </summary>
public class BlobStorageOptions
{
    /// <summary>
    /// The container name for blob storage.
    /// </summary>
    public required string ContainerName { get; set; }
}
