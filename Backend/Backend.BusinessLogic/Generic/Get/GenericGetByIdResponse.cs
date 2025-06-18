using Backend.BusinessLogic.Response;
using Backend.DataAbstraction;

namespace Backend.BusinessLogic.Generic.Get
{
  public class GenericGetByIdResponse<TDto> : GetResponse<TDto>
    where TDto : IDtoRepresentation
  {
    public GenericGetByIdResponse(TDto dto) : base(dto) { }
    public GenericGetByIdResponse():base() { }
  }
}
