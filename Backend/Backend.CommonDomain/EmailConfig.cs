namespace Backend.Services
{
  public class EmailConfig : IEmailConfig
  {
    public string From { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
  }
}
