namespace Backend.DataAbstraction
{
  public interface IAuthService
  {
    string GenerateAccessToken(string userId, string email, string role);
    string GenerateRefreshToken();
    string SendResetPassword(string userEmail, string userRole);

  }
}
