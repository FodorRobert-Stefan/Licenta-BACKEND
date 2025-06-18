using Backend.BusinessLogic.Response;

namespace Backend.BusinessLogic.Generic.Create
{
  public class GenericCreateResponse : CreateResponse
  {
    public GenericCreateResponse(string id) : base(id)
    {
    }

    public GenericCreateResponse() : base(string.Empty)
    {
    }
  }
}
