using System.Net;

namespace Backend.BusinessLogic.Exception
{
  public class RepositorieException : BaseException
  {
    public RepositorieException(HttpStatusCode statusCode, string message)
        : base(statusCode, message)
    {
    }

  }
}
