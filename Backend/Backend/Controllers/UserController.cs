using Backend.BusinessLogic.Generic.Get;
using Backend.CommonDomain.UserCommon;
using Backend.DataAbstraction;
using Backend.DataAbstraction.IAzure;
using Backend.Domain.UserDomain;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TM.Translate.Service.Api.Extensions;

namespace Backend.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IHashingGenerator hashingGenerator;
    private readonly IAzureService azureService;
    private readonly IAzureConfig azureConfig;
    public UserController(IMediator mediator, IHashingGenerator hashingGenerator, IAzureService azureService, IAzureConfig azureConfig)
    {
      this._mediator = mediator;
      this.hashingGenerator = hashingGenerator;
      this.azureService = azureService;
      this.azureConfig = azureConfig;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequestDto request, CancellationToken cancellationToken)
    {
      var getUserRequest = new GenericGetByFilterRequest<GetUserDto, User>(
        Builders<User>.Filter.Eq(x=>x.Email ,request.Email)
      );

      var response = await _mediator.Send(getUserRequest, cancellationToken);

      if (response.Data == null || !this.hashingGenerator.VerifyPassword(request.Password, response.Data.Salt, response.Data.Password))
      {
        return this.ReturnResponse(response);
      }
      var azureTokens = await this.azureService.GenerateUserAccessTokenAsync(this.azureConfig.AppendVerifiedDomainWithEmail(request.Email),request.Password);
     
      return Ok(azureTokens);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
    {
      if (string.IsNullOrWhiteSpace(request.RefreshToken))
        return BadRequest("Refresh token is required.");

      try
      {
        var newTokens = await this.azureService.RefreshUserAccessTokenAsync(request.RefreshToken);
        return Ok(newTokens);
      }
      catch (HttpRequestException ex)
      {
        return StatusCode(500, new { error = "Token refresh failed", details = ex.Message });
      }
    }

  }
}
