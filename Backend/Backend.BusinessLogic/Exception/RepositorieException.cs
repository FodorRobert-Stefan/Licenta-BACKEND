using System.Net;

namespace Backend.BusinessLogic.Exception
{
  public class RepositorieException : ServiceException
  {
    public RepositorieException(HttpStatusCode statusCode, string message)
        : base(statusCode, message)
    {
    }

  }
}
