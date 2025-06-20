using AutoMapper;
using Backend.BusinessLogic.Exception;
using Backend.DataAbstraction;
using MediatR;

namespace Backend.BusinessLogic.Generic.Create
{
  public class GenericCreateHandler<TDto, TEntity> : IRequestHandler<GenericCreateRequest<TDto, TEntity>, GenericCreateResponse>
       where TDto : IDtoRepresentation
    where TEntity : IDatabaseEntityRepresentation
  {
    private readonly IMapper mapper;
    private IGenericCrudRepository<TEntity> GenericCrudRepository;
    public GenericCreateHandler(IGenericCrudRepository<TEntity> genericCrudRepository, IMapper mapper)
    {
      GenericCrudRepository = genericCrudRepository ?? throw new DependencyException<IGenericCrudRepository<TEntity>>(typeof(GenericCreateHandler<TDto, TEntity>));
      this.mapper = mapper ?? throw new DependencyException<IMapper>(typeof(GenericCreateHandler<TDto, TEntity>));
    }

    public async Task<GenericCreateResponse> Handle(GenericCreateRequest<TDto, TEntity> request, CancellationToken cancellationToken)
    {
      try
      {
        string id = await this.GenericCrudRepository.InsertAsync(this.mapper.Map<TEntity>(request.Data), cancellationToken);
        return new GenericCreateResponse(id);
      }
      catch (RepositorieException e)
      {
        return (GenericCreateResponse)e.GetResponse();

      }
    }
  }
}
