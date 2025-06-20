using Backend.BusinessLogic.Generic.Create;
using Backend.CommonDomain.UserCommon;
using Backend.Controllers;
using Backend.DataAbstraction;
using Backend.Domain.UserDomain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class GenericControllerTests
{
  private readonly Mock<IMediator> _mediatorMock = new();
  private readonly Mock<IAzureService> _azureMock = new();
  private readonly Mock<IHashingGenerator> _hashingMock = new();
  private readonly GenericController _controller;

  public GenericControllerTests()
  {
    _hashingMock.Setup(h => h.GenerateSalt(32)).Returns("somesalt");
    _hashingMock.Setup(h => h.HashPassword(It.IsAny<string>(), "somesalt"))
                .Returns("hashed123");

    _controller = new GenericController(
      _mediatorMock.Object,
      _hashingMock.Object,
      _azureMock.Object,
      new UserModifierFactory(_hashingMock.Object)
    );
  }

  [Fact]
  public async Task CreateUser_ShouldReturn201_WhenSuccess()
  {
    var dto = new CreateUserDto
    {
      Email = "frodo@test.com",
      Password = "Frodo123!"
    };

    var response = new GenericCreateResponse("abc123")
    {
      HttpStatusCode = System.Net.HttpStatusCode.Created
    };

    _mediatorMock.Setup(m => m.Send(
      It.IsAny<GenericCreateRequest<CreateUserDto, User>>(),
      It.IsAny<CancellationToken>()
    )).ReturnsAsync(response);

    var result = await _controller.CreateUser(dto, CancellationToken.None) as ObjectResult;

    Assert.NotNull(result);
    Assert.Equal(201, result.StatusCode);

    var actualResponse = result.Value as GenericCreateResponse;
    Assert.NotNull(actualResponse);
    Assert.Equal("abc123", actualResponse.Id);
    Assert.Equal(System.Net.HttpStatusCode.Created, actualResponse.HttpStatusCode);

    _azureMock.Verify(a => a.CreateUserAsync(dto.Email, dto.Email, dto.Password), Times.Once);
  }
}
