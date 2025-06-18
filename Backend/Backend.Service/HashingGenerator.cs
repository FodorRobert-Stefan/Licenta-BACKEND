using System;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Domain.UserDomain
{
  public class HashingGenerator : IHashingGenerator
  {
    public string GenerateSalt(int size = 32)
    {
      var saltBytes = new byte[size];
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(saltBytes);
      }

      return Convert.ToBase64String(saltBytes);
    }

    public string HashPassword(string password, string salt)
    {
      var saltBytes = Convert.FromBase64String(salt);
      using (var hmac = new HMACSHA512(saltBytes))
      {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = hmac.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
      }
    }

    public bool VerifyPassword(string password, string salt, string expectedHash)
    {
      var actualHash = HashPassword(password, salt);
      return actualHash == expectedHash;
    }
    public string GenerateRandomUniqueHash(string email, string role)
    {
      string combinedInput = $"{email}:{role}:{DateTime.UtcNow.Ticks}";
      string salt = GenerateSalt();
      return HashPassword(combinedInput, salt);
    }

  }
}
