using Backend.BusinessLogic.Generic.Create;
using Backend.DataAbstraction;
using FluentValidation;

public class GenericCreateValidator<TDto, TEntity>
  : AbstractValidator<GenericCreateRequest<TDto, TEntity>>
  where TDto : IDtoRepresentation
  where TEntity : IDatabaseEntityRepresentation
{
  public GenericCreateValidator()
  {
    RuleFor(request => request.Data).NotEmpty();
  }
}
