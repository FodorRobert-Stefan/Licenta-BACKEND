namespace Backend.DataAbstraction.IAzure
{
  public interface IAzureConfig
  {
    string TenantId { get; }
    string ClientId { get; }
    string ClientSecret { get; }
    string Scope { get; }
    string GraphApiBaseUrl { get; }
    string VerifiedDomain { get; }

    public string AppendVerifiedDomain(string userPrincipalName);
    public string AppendVerifiedDomainWithEmail(string email);

    public string RemoveDomain(string userPrincipalName);
  }

}
