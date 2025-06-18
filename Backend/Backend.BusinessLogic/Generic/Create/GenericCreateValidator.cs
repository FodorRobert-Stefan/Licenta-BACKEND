using Backend.DataAbstraction;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.BusinessLogic.Generic.Create
{
  public class GenericCreateValidator<TDto, TEntity>
    : AbstractValidator<GenericCreateRequest<TDto, TEntity>>
    where TDto : IDtoRepresentation
    where TEntity : IDatabaseEntityRepresentation
  {
    public GenericCreateValidator(IServiceProvider serviceProvider)
    {
      var dtoValidator = serviceProvider.GetService<IValidator<TDto>>();

      RuleFor(x => x.Data)
        .NotNull().WithMessage("Data must not be null.");

      if (dtoValidator is not null)
      {
        RuleFor(x => x.Data).SetValidator(dtoValidator);
      }
    }
  }
}
