namespace Backend.Domain.UserDomain
{
  public interface IHashingGenerator
  {
    string GenerateSalt(int size = 32);
    string HashPassword(string password, string salt);
    bool VerifyPassword(string password, string salt, string expectedHash);
    string GenerateRandomUniqueHash(string email, string role);
  }
}
