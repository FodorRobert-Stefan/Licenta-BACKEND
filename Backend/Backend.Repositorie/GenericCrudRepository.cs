using Backend.BusinessLogic.Exception;
using Backend.DataAbstraction;
using Backend.DataAbstraction.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net;
using TM.Translate.Service.Api.Extensions;

namespace Backend.Database
{
  public class MongoGenericRepository<T> : IGenericCrudRepository<T> where T : IDatabaseEntityRepresentation
  {
    private readonly IMongoCollection<T> _collection;

    public MongoGenericRepository(IDatabase database, ILogger<MongoGenericRepository<T>> logger)
    {
      _collection = database.GetCollection<T>()
     ?? throw new DependencyException<IDatabase>(typeof(MongoGenericRepository<T>));
    }
    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
      var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
      return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
      return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<string> InsertAsync(T entity, CancellationToken cancellationToken, Action<T>? onBeforeInsert = null)
    {
      onBeforeInsert?.Invoke(entity);
      await _collection.InsertOneAsync(entity);
      if (entity != null && entity.Id != null && entity.Id.IsValidObjectId() == false)
      {
        throw new RepositorieException(System.Net.HttpStatusCode.InternalServerError, HttpStatusCode.InternalServerError.GetDescription());
      }
      return entity.Id;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
      var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
      await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
      var filter = Builders<T>.Filter.Eq(e => e.Id, id);
      await _collection.DeleteOneAsync(filter);
    }

    public async Task<T> FindByFilterAsync(FilterDefinition<T> filter, CancellationToken cancellationToken)
    {
      try
      {
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
      }
      catch (Exception ex)
      {
        throw new RepositorieException(
          HttpStatusCode.InternalServerError,
          $"Mongo filter query failed: {ex.Message}"
        );
      }
    }
  }
}
