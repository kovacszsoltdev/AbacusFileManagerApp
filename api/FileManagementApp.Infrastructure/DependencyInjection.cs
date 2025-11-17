using FileManagementApp.Application.Common.Interfaces;
using FileManagementApp.Infrastructure.Files;
using FileManagementApp.Infrastructure.Options;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileManagementApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BlobStorageOptions>(options =>
        {
            options.ContainerName = configuration["AZURE_STORAGE_CONTAINER_NAME"]
                ?? throw new InvalidOperationException("Missing configuration: AZURE_STORAGE_CONTAINER_NAME");
        });

        services.AddScoped<IFileService, BlobFileService>();        

        services.AddAzureClients(builder =>
        {

            var conn = configuration["AZURE_STORAGE_CONNECTION_STRING"]
                ?? throw new InvalidOperationException("Missing configuration: AZURE_STORAGE_CONNECTION_STRING");
            builder.AddBlobServiceClient(conn);
        });

        return services;
    }
}
