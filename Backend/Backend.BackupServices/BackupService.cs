using Backend.DataAbstraction;
using Backend.Service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

public class BackupService : BackgroundService
{
  private readonly AzureBlobStorageSettings _blobSettings;
  private readonly IAzureBlobBackupService _blobBackupService;
  private readonly string MongoConnectionString ;
  private readonly string MongoDbName;
  private static readonly string[] Collections = ["User"];
  private const string OutputPath = "./backups";

  public BackupService(IOptions<AzureBlobStorageSettings> options, IAzureBlobBackupService blobBackupService, IDatabaseConfiguration databaseConfiguration)
  {
    _blobSettings = options.Value;
    _blobBackupService = blobBackupService;
    this.MongoConnectionString = databaseConfiguration.ConnectionString;
    this.MongoDbName = databaseConfiguration.DatabaseName;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      var todayFolder = DateTime.Now.ToString("yyyy-MM-dd");
      var fullOutputPath = Path.Combine(OutputPath, todayFolder);
      Directory.CreateDirectory(fullOutputPath);

      foreach (var collectionName in Collections)
      {
        var filePath = Path.Combine(fullOutputPath, $"{collectionName}.json");
        await BackupCollectionAsync(MongoConnectionString, MongoDbName, collectionName, fullOutputPath);
        await _blobBackupService.UploadWithRetryAsync(filePath, $"{todayFolder}/{collectionName}-{DateTime.UtcNow:HHmmss}.json");
      }

      await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
    }
  }


  private async Task BackupCollectionAsync(string connectionString, string dbName, string collectionName, string outputPath)
  {
    var client = new MongoClient(connectionString);
    var db = client.GetDatabase(dbName);
    var collection = db.GetCollection<BsonDocument>(collectionName);
    var documents = await collection.Find(_ => true).ToListAsync();

    var json = documents.ToJson();
    await File.WriteAllTextAsync(Path.Combine(outputPath, $"{collectionName}.json"), json);
  }
}
