using Backend.DataAbstraction;
using Backend.Domain.UserDomain;
using Backend.Domain;
using MongoDB.Driver;
using Backend.BusinessLogic.Exception;

public class UserRepository : IUserRepository
{
  private readonly IAuthService authService;
  private readonly IHashingGenerator hashingGenerator;
  private readonly IMongoCollection<User> userCollection;

  public UserRepository(
    IAuthService authService,
    IHashingGenerator hashingGenerator,
    IDatabase database)
  {
    if (database == null)
    {
      throw new DependencyException<IDatabase>(typeof(IUserRepository));
    }
    this.authService = authService;
    this.hashingGenerator = hashingGenerator;
    this.userCollection = database.GetCollection<User>() ?? throw new DependencyException<IDatabase>(typeof(IUserRepository));
  }

  public bool Login(string email, string password)
  {
    var user = userCollection.Find(u => u.Email == email).FirstOrDefault();
    if (user == null)
      throw new RepositorieException(System.Net.HttpStatusCode.BadRequest, "User not found");

    var hash = hashingGenerator.HashPassword(password, user.Security.Salt);

    if (user.Security.LoginRetryCount >= 5)
      throw new RepositorieException(System.Net.HttpStatusCode.BadRequest, "Too many failed login attempts.");

    if (hash != user.Password)
    {
      user.Security.LoginRetryCount++;
      userCollection.ReplaceOne(u => u.Id == user.Id, user);
      return false;
    }

    user.Security.LoginRetryCount = 0;

    user.Security.AccessToken = authService.GenerateAccessToken(user.Id, user.Email, "User");
    user.Security.RefreshToken = authService.GenerateRefreshToken();

    userCollection.ReplaceOne(u => u.Id == user.Id, user);
    return true;
  }
  public void ResetPassword(string email, string newPassword)
  {
    var user = userCollection.Find(u => u.Email == email).FirstOrDefault();
    if (user == null)
      throw new RepositorieException(System.Net.HttpStatusCode.BadRequest, "Invalid User Email");
    user.SetPassword(newPassword, hashingGenerator);
    user.Security.LoginRetryCount = 0;
    user.Security.AccessToken = null;
    user.Security.RefreshToken = null;

    userCollection.ReplaceOne(u => u.Id == user.Id, user);
  }

}
