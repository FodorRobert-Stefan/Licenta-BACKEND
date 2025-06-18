using System.Net;

namespace Backend.BusinessLogic.Response
{
  public class Response
  {
    public string Message { get; set; }

    public HttpStatusCode HttpStatusCode { get; set; }

    public Response()
    {

    }

    public Response(string message, HttpStatusCode httpStatusCode)
    {
      Message = message;
      HttpStatusCode = httpStatusCode;
    }
  }
}
