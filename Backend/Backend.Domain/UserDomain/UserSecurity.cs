using Backend.CommonDomain;

namespace Backend.Domain.UserDomain
{
  public class Security : InactiveEntity
  {
    public string Salt { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public ushort LoginRetryCount { get; set; } = 0;

    public Security()
    { 
    }

  }
}
