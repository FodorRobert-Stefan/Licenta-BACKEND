
using Backend.DataAbstraction;
using MongoDB.Driver;

public interface IDatabase
{
  IMongoCollection<TClass>? GetCollection<TClass>()
      where TClass : IDatabaseEntityRepresentation;
}
