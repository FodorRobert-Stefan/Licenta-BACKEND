using Backend.BusinessLogic.Generic.Create;
using Backend.BusinessLogic.Generic.Get;
using Backend.CommonDomain.UserCommon;
using Backend.DataAbstraction;
using Backend.Domain.UserDomain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TM.Translate.Service.Api.Extensions;

namespace Backend.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class GenericController : ControllerBase
  {
    private readonly IMediator mediator;
    private readonly IHashingGenerator hashingGenerator;
    private readonly IAzureService azureService;

    public GenericController(IMediator mediator, IHashingGenerator hashingGenerator, IAzureService azureService)
    {
      this.mediator = mediator;
      this.hashingGenerator = hashingGenerator;
      this.azureService = azureService;
    }

    [HttpPost(nameof(User))]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto request, CancellationToken cancellationToken)
    {
      GenericCreateRequest<CreateUserDto, User> createRequest = new(request, user =>
      {
        var salt = hashingGenerator.GenerateSalt();
        user.Security ??= new();
        user.Security.Salt = salt;
        user.Password = hashingGenerator.HashPassword(user.Password, salt);
      });
      var response = await this.mediator.Send(createRequest, cancellationToken);
      await this.azureService.CreateUserAsync(request.Email, request.Email, request.Password);
      return this.ReturnResponse(response);
    }
    [HttpGet(nameof(User)+"/{id}")]
    public async Task<IActionResult> CreateUser(string id, CancellationToken cancellationToken)
    {
      GenericGetByIdRequest<CreateUserDto, User> getRequest = new(id);
      var response = await this.mediator.Send(getRequest, cancellationToken);
      return this.ReturnResponse(response);
    }
  }
}
