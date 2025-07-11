﻿using Backend.CommonDomain.UserCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.DataAbstraction
{
  public interface IAzureService
  {
    Task<string> GenerateAccessTokenAsync();
    Task<string> CreateUserAsync(string displayName, string userPrincipalName, string password);
    Task AssignRoleAsync(string userId, string roleId);
    Task<TokenResponse> GenerateUserAccessTokenAsync(string username, string password);
    Task<TokenResponse> RefreshUserAccessTokenAsync(string refreshToken);

  }

}
