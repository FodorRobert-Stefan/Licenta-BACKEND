using Backend.DataAbstraction;
using MediatR;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Backend.BusinessLogic.Generic.Get
{
  public class GenericGetByFilterRequest<TDto, TEntity> : IRequest<GenericGetByFilterResponse<TDto>>
    where TDto : IDtoRepresentation
    where TEntity : IDatabaseEntityRepresentation
  {
    public FilterDefinition<TEntity> Filter { get; }

    public GenericGetByFilterRequest(FilterDefinition<TEntity> filter)
    {
      this.Filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }
  }
}
