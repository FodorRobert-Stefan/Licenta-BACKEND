using Backend.BusinessLogic.Response;
using Backend.DataAbstraction;

namespace Backend.BusinessLogic.Generic.Get
{
  public class GenericGetByFilterResponse<TDto> : GetResponse<TDto>
    where TDto : IDtoRepresentation
  {
    public GenericGetByFilterResponse(TDto dto) : base(dto) { }
    public GenericGetByFilterResponse() : base() { }
  }
}
