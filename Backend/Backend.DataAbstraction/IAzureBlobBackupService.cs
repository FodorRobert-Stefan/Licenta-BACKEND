namespace Backend.DataAbstraction
{
  public interface IAzureBlobBackupService
  {
    Task UploadWithRetryAsync(string filePath, string blobName);
  }
}
