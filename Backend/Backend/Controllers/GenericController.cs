using Backend.BusinessLogic.Exception;
using Backend.BusinessLogic.Generic.Create;
using Backend.BusinessLogic.Generic.Get;
using Backend.BusinessLogic.Response;
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
    private readonly UserModifierFactory modifierFactory;

    public GenericController(IMediator mediator, IHashingGenerator hashingGenerator, IAzureService azureService, UserModifierFactory userModifierFactory)
    {
      this.mediator = mediator;
      this.hashingGenerator = hashingGenerator;
      this.azureService = azureService;
      this.modifierFactory = userModifierFactory;
    }

    [HttpPost(nameof(User))]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto request, CancellationToken cancellationToken)
    {
      var modifier = this.modifierFactory.CreateModifier(request.Password);
      var createRequest = new GenericCreateRequest<CreateUserDto, User>(request, modifier);
      var response = await this.mediator.Send(createRequest, cancellationToken);
      await this.azureService.CreateUserAsync(request.Email, request.Email, request.Password);
      return this.ReturnResponse(response);
    }
    [HttpGet(nameof(User) + "/{id}")]
    public async Task<IActionResult> CreateUser(string id, CancellationToken cancellationToken)
    {
      GenericGetByIdRequest<CreateUserDto, User> getRequest = new(id);
      var response = await this.mediator.Send(getRequest, cancellationToken);
      return this.ReturnResponse(response);
    }
  }
}
