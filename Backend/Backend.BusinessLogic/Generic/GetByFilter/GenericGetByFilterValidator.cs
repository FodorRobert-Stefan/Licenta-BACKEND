using Backend.BusinessLogic.Generic.Get;
using Backend.DataAbstraction;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.BusinessLogic.Generic.GetByFilter
{
  public class GenericGetByFilterValidator<TDto, TEntity> : AbstractValidator<GenericGetByFilterRequest<TDto, TEntity>>
    where TEntity : IDatabaseEntityRepresentation
    where TDto : IDtoRepresentation
  {
    public GenericGetByFilterValidator(IServiceProvider serviceProvider)
    {
      this.RuleFor(request => request.Filter).NotEmpty();
    }
  }
}
