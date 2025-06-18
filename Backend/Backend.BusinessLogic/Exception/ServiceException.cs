using System.Net;

namespace Backend.BusinessLogic
{
  public class ServiceException : System.Exception
  {
    public HttpStatusCode StatusCode { get; }

    public ServiceException(HttpStatusCode statusCode, string message)
        : base(message)
    {
      StatusCode = statusCode;
    }

    public ServiceException(HttpStatusCode statusCode, string message, System.Exception innerException)
        : base(message, innerException)
    {
      StatusCode = statusCode;
    }
  }
}
