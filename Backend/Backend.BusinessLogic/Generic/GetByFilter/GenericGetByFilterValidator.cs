using Backend.BusinessLogic.Generic.Get;
using Backend.DataAbstraction;
using FluentValidation;

namespace Backend.BusinessLogic.Generic.GetByFilter
{
  public class GenericGetByFilterValidator<TDto, TEntity> : AbstractValidator<GenericGetByFilterRequest<TDto, TEntity>>
    where TEntity : IDatabaseEntityRepresentation
    where TDto : IDtoRepresentation
  {
    public GenericGetByFilterValidator()
    {
      this.RuleFor(request => request.Filter).NotEmpty();
    }
  }
}
