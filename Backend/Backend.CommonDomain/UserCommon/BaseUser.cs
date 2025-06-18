namespace Backend.CommonDomain.UserCommon
{
  public class BaseUser : InactiveEntity
    {
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

  }
}
