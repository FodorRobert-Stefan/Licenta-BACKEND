using Backend.DataAbstraction;

namespace Backend.CommonDomain.UserCommon
{
  public class LoginUserRequestDto : IDtoRepresentation
  {
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
  }
}
