using Backend.DataAbstraction;
using MongoDB.Driver;

public class MongoDatabase : IDatabase
{
  private readonly IMongoDatabase _database;

  public MongoDatabase(IDatabaseConfiguration config)
  {
    var client = new MongoClient(config.ConnectionString);
    _database = client.GetDatabase(config.DatabaseName);
  }

  public IMongoCollection<TClass> GetCollection<TClass>()
    where TClass : IDatabaseEntityRepresentation
  {
    var collectionName = typeof(TClass).Name;
    return _database.GetCollection<TClass>(collectionName);
  }
}
