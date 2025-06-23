using Backend.DataAbstraction;
using Backend.DataAbstraction.IAzure;

namespace Backend.CommonDomainu
{
  public class AzureConfig : IAzureConfig
  {
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string GraphApiBaseUrl { get; set; } = string.Empty;
    public string VerifiedDomain { get; set; } = string.Empty;

    public string AppendVerifiedDomain(string userPrincipalName)
    {
      return $"{userPrincipalName}@{this.VerifiedDomain}";
    }
    public string AppendVerifiedDomainWithEmail(string email)
    {
      var atIndex = email.LastIndexOf('@');
      if (atIndex > 0)
      {
        return $"{email.Substring(0, atIndex)}@{this.VerifiedDomain}";
      }
      return email;
    }
    public string RemoveDomain(string userPrincipalName)
    {
      var atIndex = userPrincipalName.LastIndexOf('@');
      return atIndex > 0 ? userPrincipalName.Substring(0, atIndex) : userPrincipalName;
    }

  }
}
