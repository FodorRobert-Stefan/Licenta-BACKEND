using Backend.DataAbstraction;

namespace Backend.Domain.UserDomain
{
  public interface IUserRepository : IRepository
  {
    public bool Login(string username, string password);
    public void ResetPassword(string email, string newPassword);

  }
}
