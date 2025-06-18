namespace Backend.BusinessLogic.Request
{
  public class GetRequest
  {
    public string Id { get; set; } = string.Empty;
    public GetRequest(string id)
    {
      this.Id = id;
    }
  }
}
