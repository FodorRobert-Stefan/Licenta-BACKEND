using Backend.DataAbstraction;

namespace Backend.BusinessLogic.Request
{
  public class CreateRequest<TDto>
    where TDto : IDtoRepresentation
  {
    public TDto? Data { get; set; }
    public CreateRequest(TDto data)
    {
      Data = data;
    }
  }
}
