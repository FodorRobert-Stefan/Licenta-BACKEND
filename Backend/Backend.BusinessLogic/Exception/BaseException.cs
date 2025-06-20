using System.Net;
using Backend.BusinessLogic.Response;

namespace Backend.BusinessLogic
{
  public class BaseException : System.Exception
  {
    public HttpStatusCode StatusCode { get; private set; }

    public BaseException(HttpStatusCode statusCode, string message)
        : base(message)
    {
      StatusCode = statusCode;
    }

    public BaseException(HttpStatusCode statusCode, string message, System.Exception innerException)
        : base(message, innerException)
    {
      StatusCode = statusCode;
    }
    public Response.Response GetResponse()
    {
      return new Response.Response(Message, StatusCode);
    }
  }
}
