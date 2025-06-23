using Backend.DataAbstraction;
using System.Net;
using TM.Translate.Service.Api.Extensions;

namespace Backend.BusinessLogic.Response
{
  public class GetResponse<T> : Response
    where T : IDtoRepresentation
  {
    public T? Data { get; set; }
    public GetResponse(T? data) : base(data != null ? HttpStatusCode.OK.GetDescription() : HttpStatusCode.BadRequest.GetDescription(), HttpStatusCode.OK)
    {
      this.Data = data;
    }
    public GetResponse() : base(HttpStatusCode.BadRequest.GetDescription(), HttpStatusCode.BadRequest)
    {

    }
  }
}
