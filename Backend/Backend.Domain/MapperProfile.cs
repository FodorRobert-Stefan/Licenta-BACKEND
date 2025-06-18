using AutoMapper;
using Backend.CommonDomain.UserCommon;

public class AutoMapperProfile : Profile
{
  public AutoMapperProfile()
  {
    CreateMap<CreateUserDto, User>();
    CreateMap<User, CreateUserDto>();
    CreateMap<User, GetUserDto>().AfterMap((src, dest) =>
    {
      dest.Salt = src.Security.Salt;
    });
    CreateMap<GetUserDto, User>();
  }
}
