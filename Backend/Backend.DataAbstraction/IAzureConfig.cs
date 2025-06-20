using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DataAbstraction
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
    public string RemoveDomain(string userPrincipalName);
  }

}
