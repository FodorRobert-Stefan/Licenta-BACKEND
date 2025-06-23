using AutoMapper;
using Backend.BusinessLogic.Exception;
using Backend.DataAbstraction;
using MediatR;

namespace Backend.BusinessLogic.Generic.Get
{
  public class GenericGetByFilterHandler<TDto, TEntity>
    : IRequestHandler<GenericGetByFilterRequest<TDto, TEntity>, GenericGetByFilterResponse<TDto>>
    where TEntity : IDatabaseEntityRepresentation
    where TDto : IDtoRepresentation
  {
    private readonly IGenericCrudRepository<TEntity> genericCrudRepository;
    private readonly IMapper mapper;

    public GenericGetByFilterHandler(IGenericCrudRepository<TEntity> genericCrudRepository, IMapper mapper)
    {
      this.genericCrudRepository = genericCrudRepository;
      this.mapper = mapper;
    }

    public async Task<GenericGetByFilterResponse<TDto>> Handle(GenericGetByFilterRequest<TDto, TEntity> request, CancellationToken cancellationToken)
    {
      try
      {
        var entity = await genericCrudRepository.FindByFilterAsync(request.Filter, cancellationToken);
        if (entity == null)
        {
          return new GenericGetByFilterResponse<TDto>();
        }
        var mapped = mapper.Map<TDto>(entity);

        return new GenericGetByFilterResponse<TDto>(mapped);
      }
      catch (RepositorieException e)
      {
        return (GenericGetByFilterResponse<TDto>)e.GetResponse();
      }
    }
  }
}
