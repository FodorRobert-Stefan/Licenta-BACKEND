using Backend.BusinessLogic.Generic.Create;
using Backend.BusinessLogic.Request;
using Backend.DataAbstraction;
using MediatR;

namespace Backend.BusinessLogic.Generic.Get
{
  public class GenericGetByIdRequest<TDto, TEntity> : GetRequest, IRequest<GenericGetByIdResponse<TDto>>
    where TDto : IDtoRepresentation
    where TEntity : IDatabaseEntityRepresentation
  {
    public GenericGetByIdRequest(string id) : base(id)
    {
    }
  }
}
