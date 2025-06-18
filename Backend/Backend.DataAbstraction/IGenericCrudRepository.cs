using MongoDB.Driver;

namespace Backend.DataAbstraction
{
  public interface IGenericCrudRepository<T> where T : IDatabaseEntityRepresentation
  {
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<string> InsertAsync(T entity, CancellationToken cancellationToken, Action<T>? onBeforeInsert = null);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
    public Task<T> FindByFilterAsync(FilterDefinition<T> filter, CancellationToken cancellationToken);
  }
}
