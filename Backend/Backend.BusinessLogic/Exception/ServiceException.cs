using System.Net;

namespace Backend.BusinessLogic
{
  public class ServiceException : BaseException
  {
    public ServiceException(HttpStatusCode statusCode, string message)
        : base(statusCode, message)
    {
    }
  }
}
