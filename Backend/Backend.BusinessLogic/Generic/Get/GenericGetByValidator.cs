using Backend.DataAbstraction;
using Backend.DataAbstraction.Extensions;
using FluentValidation;

namespace Backend.BusinessLogic.Generic.Get
{
  public class GenericGetByValidator<TEntity, TDto> : AbstractValidator<GenericGetByIdRequest<TDto, TEntity>>
    where TEntity : IDatabaseEntityRepresentation
    where TDto : IDtoRepresentation
  {
    public GenericGetByValidator()
    {
      this.RuleFor(request => request.Id.IsValidObjectId()).Equal(true);
    }
  }
}
