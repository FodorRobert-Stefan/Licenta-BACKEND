using AutoMapper;
using Backend.BusinessLogic.Exception;
using Backend.DataAbstraction;
using MediatR;

namespace Backend.BusinessLogic.Generic.Get
{
  public class GenericGetByIdHandler<TEntity, TDto> : IRequestHandler<GenericGetByIdRequest<TDto, TEntity>, GenericGetByIdResponse<TDto>>
    where TEntity : IDatabaseEntityRepresentation
    where TDto : IDtoRepresentation
  {
    private readonly IGenericCrudRepository<TEntity> genericCrudRepository;
    private readonly IMapper mapper;

    public GenericGetByIdHandler(IGenericCrudRepository<TEntity> genericCrudRepository, IMapper mapper)
    {
      this.mapper = mapper;
      this.genericCrudRepository = genericCrudRepository;
    }
    public async Task<GenericGetByIdResponse<TDto>> Handle(GenericGetByIdRequest<TDto, TEntity> request, CancellationToken cancellationToken)
    {
      try
      {
        var mappedDocument = this.mapper.Map<TDto>((await this.genericCrudRepository.GetByIdAsync(request.Id, cancellationToken)));
        return new GenericGetByIdResponse<TDto>(mappedDocument);
      }
      catch (RepositorieException e)
      {
        return new GenericGetByIdResponse<TDto>
        {
          HttpStatusCode = e.StatusCode,
          Message = e.Message,
        };
      }
    }
  }
}
