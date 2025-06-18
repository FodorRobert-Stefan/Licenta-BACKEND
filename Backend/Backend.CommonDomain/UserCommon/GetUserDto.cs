using Backend.DataAbstraction;
using System.Text.Json.Serialization;

namespace Backend.CommonDomain.UserCommon
{
  public class GetUserDto : IDtoRepresentation
  {
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    [JsonIgnore]
    public string Password { get; set; } = string.Empty;
    [JsonIgnore]
    public string Salt {  get; set; } = string.Empty;
  }
}
