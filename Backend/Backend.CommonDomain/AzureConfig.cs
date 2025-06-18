using Backend.DataAbstraction;

namespace Backend.CommonDomain
{

  public class AzureConfig : IAzureConfig
  {
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string GraphApiBaseUrl { get; set; } = string.Empty;
  }

}
