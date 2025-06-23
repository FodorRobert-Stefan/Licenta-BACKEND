using Backend.Database;

namespace Backend.CommonDomain
{
  public class JwtConfig : IJwtConfig
  {
    public string Secret { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;
    public int AccessTokenExpirationMinutes { get; set; } = 30;

  }
}
