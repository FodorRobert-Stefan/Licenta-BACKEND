using Backend.BusinessLogic.Generic.Create;
using Backend.BusinessLogic.Generic.Get;
using Backend.CommonDomain.UserCommon;
using FluentValidation;
using MediatR;

namespace Backend.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
      services.AddTransient<IRequestHandler<GenericCreateRequest<CreateUserDto, User>, GenericCreateResponse>, GenericCreateHandler<CreateUserDto, User>>();
      services.AddTransient<IRequestHandler<GenericGetByIdRequest<CreateUserDto, User>, GenericGetByIdResponse<CreateUserDto>>, GenericGetByIdHandler<User, CreateUserDto>>();
      services.AddTransient<IRequestHandler<GenericGetByFilterRequest<GetUserDto, User>, GenericGetByFilterResponse<GetUserDto>>, GenericGetByFilterHandler<GetUserDto, User>>();

      services.AddTransient<IValidator<CreateUserDto>, CreateUserDtoValidatorBase>();
      services.AddTransient<IValidator<GenericCreateRequest<CreateUserDto, User>>, GenericCreateValidator<CreateUserDto, User>>();

      services.AddScoped<UserModifierFactory>();

      return services;
    }
  }
}
