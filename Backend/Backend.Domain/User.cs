using Backend.DataAbstraction;
using Backend.Domain.UserDomain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User : IDatabaseEntityRepresentation
{
  public string Email { get; set; }

  public Security Security { get; set; }
  public string Password { get; set; } = string.Empty;
  public string? Id { get; set; } = null;

  public void SetPassword(string rawPassword, IHashingGenerator hashingGenerator)
  {
    if (string.IsNullOrWhiteSpace(rawPassword))
      throw new ArgumentException("Password must not be empty");

    this.Password = hashingGenerator.HashPassword(rawPassword, this.Security.Salt);
  }
  public static class AvailableFields
  {
    public const string Id = "Id";
    public const string Email = "Email";
    public const string Password = "Password";

    public static class Security
    {
      public const string Root = "Security";
      public const string Salt = "Salt";
      public const string AccessToken = "AccessToken";
      public const string RefreshToken = "RefreshToken";
      public const string LoginRetryCount = "LoginRetryCount";
    }
  }
}
