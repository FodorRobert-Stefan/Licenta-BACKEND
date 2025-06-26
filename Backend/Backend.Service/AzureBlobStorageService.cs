using Azure.Storage.Blobs;
using Backend.DataAbstraction;
using Microsoft.Extensions.Options;
using Polly;

namespace Backend.Service
{
  public class AzureBlobBackupService : IAzureBlobBackupService
  {
    private readonly AzureBlobStorageSettings _settings;

    public AzureBlobBackupService(IOptions<AzureBlobStorageSettings> options)
    {
      _settings = options.Value;
    }

    public async Task UploadWithRetryAsync(string filePath, string blobName)
    {
      var retryPolicy = Policy
          .Handle<Exception>()
          .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2));

      await retryPolicy.ExecuteAsync(async () =>
      {
        var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(blobName);
        await using var stream = File.OpenRead(filePath);
        await blobClient.UploadAsync(stream, overwrite: true);
      });
    }
  }
}
