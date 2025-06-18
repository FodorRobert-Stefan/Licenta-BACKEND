using Backend.BusinessLogic.Request;
using Backend.DataAbstraction;
using MediatR;

namespace Backend.BusinessLogic.Generic.Create
{
  public class GenericCreateRequest<TDto, TEntity> : CreateRequest<TDto>, IRequest<GenericCreateResponse>
    where TDto : IDtoRepresentation
    where TEntity : IDatabaseEntityRepresentation

  {
    public Action<TEntity>? OnBeforeInsert { get; set; }
    public GenericCreateRequest(TDto Data, Action<TEntity>? OnBeforeInsert) : base(Data)
    {
      this.OnBeforeInsert = OnBeforeInsert;
    }
  }
}
