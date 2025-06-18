using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.DataAbstraction;
using Backend.Database;
using Backend.Domain.UserDomain;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
  public class AuthService : IAuthService
  {
    private readonly IJwtConfig _configuration;
    private readonly IEmailSender emailSender;
    private readonly IHashingGenerator hashingGenerator;

    public AuthService(IJwtConfig configuration, IHashingGenerator hashingGenerator, IEmailSender emailSender)
    {
      this._configuration = configuration;
      this.hashingGenerator = hashingGenerator;
      this.emailSender = emailSender;
    }

    public string GenerateAccessToken(string userId, string email, string role)
    {
      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(ClaimTypes.Role, role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      var secretKey = this._configuration.Secret;
      if (string.IsNullOrEmpty(secretKey))
      {
        throw new InvalidOperationException("Missing JWT Secret in configuration.");
      }

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        issuer: this._configuration.Issuer,
        audience:this._configuration.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(30),
        signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
      var randomBytes = new byte[64];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomBytes);
      return Convert.ToBase64String(randomBytes);
    }

    public string SendResetPassword(string userEmail, string userRole)
    {
      string hash = this.hashingGenerator.GenerateRandomUniqueHash(userEmail, userRole);

      string resetLink = $"https://yourfrontend.com/reset-password?token={hash}";

      string subject = "Resetare parolă cont";
      string htmlBody = $@"
      <p>Bună,</p>
      <p>Ai solicitat resetarea parolei pentru contul tău.</p>
      <p>Te rugăm să dai click pe linkul de mai jos pentru a-ți reseta parola:</p>
      <p><a href=""{resetLink}"">Resetează parola</a></p>
      <p>Dacă nu ai făcut această solicitare, poți ignora acest mesaj.</p>
      <p>Echipa ta</p>";

      this.emailSender.SendEmail(userEmail, subject, htmlBody);
      return hash;
    }

  }
}
