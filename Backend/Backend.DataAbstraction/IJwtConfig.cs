namespace Backend.Database
{
  public interface IJwtConfig
  {
    string Secret { set; get; }

    string Issuer { set; get; }

    string Audience { set; get; }
    int AccessTokenExpirationMinutes { set; get; }

  }
}
