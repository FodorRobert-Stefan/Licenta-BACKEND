using Backend.DataAbstraction.Extensions;
using System.Net;
using TM.Translate.Service.Api.Extensions;

namespace Backend.BusinessLogic.Response
{
  public class CreateResponse : Response
  {
    public string Id { get; set; }

    public CreateResponse():base()
    {

    }
    public CreateResponse(string id)
      : base(
          GetMessage(id, out var isValid),
          isValid ? System.Net.HttpStatusCode.Created : System.Net.HttpStatusCode.BadRequest)
    {
      Id = id;
    }

    private static string GetMessage(string id, out bool isValid)
    {
      isValid = id.IsValidObjectId();
      return isValid ? HttpStatusCode.Created.GetDescription() : HttpStatusCode.BadRequest.GetDescription();
    }
  }
}
