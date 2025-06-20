using Backend.DataAbstraction;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Backend.BusinessLogic;
using System.Net;

public class AzureService : IAzureService
{
  private readonly IAzureConfig _config;
  private readonly HttpClient _httpClient;

  public AzureService(IAzureConfig config)
  {
    _config = config;
    _httpClient = new HttpClient();
  }

  public async Task<string> GenerateAccessTokenAsync()
  {
    var tokenEndpoint = $"https://login.microsoftonline.com/{_config.TenantId}/oauth2/v2.0/token";

    var content = new FormUrlEncodedContent(new[]
    {
        new KeyValuePair<string, string>("client_id", _config.ClientId),
        new KeyValuePair<string, string>("client_secret", _config.ClientSecret),
        new KeyValuePair<string, string>("scope", _config.Scope),
        new KeyValuePair<string, string>("grant_type", "client_credentials")
    });

    var response = await _httpClient.PostAsync(tokenEndpoint, content);

    var rawContent = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
      throw new HttpRequestException($"AccessToken error ({(int)response.StatusCode}): {rawContent}");
    }

    var token = JsonDocument.Parse(rawContent).RootElement.GetProperty("access_token").GetString();
    return token!;
  }


  public async Task<string> CreateUserAsync(string displayName, string userPrincipalName, string password)
  {
    userPrincipalName = this._config.AppendVerifiedDomain(userPrincipalName);
    var token = await GenerateAccessTokenAsync();
    var userPayload = new
    {
      accountEnabled = true,
      displayName = displayName,
      mailNickname = userPrincipalName.Split('@')[0],
      userPrincipalName = userPrincipalName,
      passwordProfile = new
      {
        forceChangePasswordNextSignIn = false,
        password = password
      }
    };

    var req = new HttpRequestMessage(HttpMethod.Post, $"{_config.GraphApiBaseUrl}/users");
    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    req.Content = new StringContent(JsonSerializer.Serialize(userPayload), Encoding.UTF8, "application/json");
    var resp = await _httpClient.SendAsync(req);
    if (!resp.IsSuccessStatusCode)
    {
      var rawContent = await resp.Content.ReadAsStringAsync();

      if (resp.StatusCode == HttpStatusCode.BadRequest &&
          rawContent.Contains("password", StringComparison.OrdinalIgnoreCase) &&
          rawContent.Contains("complexity", StringComparison.OrdinalIgnoreCase))
      {
        throw new ServiceException(HttpStatusCode.BadRequest, "Password does not meet complexity requirements.");
      }
      else
      {
        throw new ServiceException(HttpStatusCode.InternalServerError, $"Failed to insert the user ({(int)resp.StatusCode}): {rawContent}");
      }
    }


    var content = await resp.Content.ReadAsStringAsync();
    var userId = JsonDocument.Parse(content).RootElement.GetProperty("id").GetString();
    return userId!;
  }

  public async Task AssignRoleAsync(string userId, string roleId)
  {
    var token = await GenerateAccessTokenAsync();

    var req = new HttpRequestMessage(
        HttpMethod.Post,
        $"{_config.GraphApiBaseUrl}/directoryRoles/{roleId}/members/$ref");

    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var payload = new
    {
      @odataid = $"{_config.GraphApiBaseUrl}/directoryObjects/{userId}"
    };
    req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

    var resp = await _httpClient.SendAsync(req);
    resp.EnsureSuccessStatusCode();
  }
}
