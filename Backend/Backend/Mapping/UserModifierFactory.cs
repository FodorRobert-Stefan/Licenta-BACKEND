using Backend.DataAbstraction;
using Backend.Domain.UserDomain;

public class UserModifierFactory
{
  private readonly IHashingGenerator hashing;

  public UserModifierFactory(IHashingGenerator hashing)
  {
    this.hashing = hashing;
  }

  public Action<User> CreateModifier(string plainPassword)
  {
    return user =>
    {
      var salt = hashing.GenerateSalt();
      user.Security ??= new();
      user.Security.Salt = salt;
      user.Password = hashing.HashPassword(plainPassword, salt);
    };
  }
}
