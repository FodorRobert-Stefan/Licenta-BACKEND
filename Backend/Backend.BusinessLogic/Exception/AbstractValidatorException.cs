using System.Net;

namespace Backend.BusinessLogic.Exception
{
  public class AbstractValidatorException : BaseException
  {
    public AbstractValidatorException(HttpStatusCode statusCode, string message)
           : base(statusCode, message)
    {
    }
  }
}
