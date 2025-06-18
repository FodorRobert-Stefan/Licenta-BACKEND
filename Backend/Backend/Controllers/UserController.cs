using Backend.BusinessLogic.Generic.Get;
using Backend.CommonDomain.UserCommon;
using Backend.Domain.UserDomain;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Backend.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IHashingGenerator hashingGenerator;

    public UserController(IMediator mediator, IHashingGenerator hashingGenerator)
    {
      this._mediator = mediator;
      this.hashingGenerator = hashingGenerator;
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
        return Unauthorized("Invalid email or password");
      }

      return Ok(response.Data);
    }
  }
}
