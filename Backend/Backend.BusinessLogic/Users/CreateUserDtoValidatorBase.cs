using System.Net;
using Backend.BusinessLogic.Exception;
using Backend.CommonDomain.UserCommon;
using FluentValidation;
using FluentValidation.Results;

public class CreateUserDtoValidatorBase : AbstractValidator<CreateUserDto>
{
  public CreateUserDtoValidatorBase()
  {
    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Invalid email format.");

    RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
        .Must(ContainUppercase).WithMessage("Password must contain at least one uppercase letter.")
        .Must(ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
        .Must(ContainDigit).WithMessage("Password must contain at least one digit.")
        .Must(ContainSpecialCharacter).WithMessage("Password must contain at least one special character.")
        .Must((dto, password) => !ContainsUsernameOrEmailPrefix(password, dto.Email))
            .WithMessage("Password must not contain parts of the email address.")
        .Must(NotContainBlacklistedWords)
            .WithMessage("Password is too common or insecure.");
  }

  public override ValidationResult Validate(ValidationContext<CreateUserDto> context)
  {
    var result = base.Validate(context);

    if (!result.IsValid)
    {
      var message = string.Join(" | ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
      throw new AbstractValidatorException(HttpStatusCode.BadRequest, message);
    }

    return result;
  }

  private bool ContainUppercase(string password) =>
      password.Any(char.IsUpper);

  private bool ContainLowercase(string password) =>
      password.Any(char.IsLower);

  private bool ContainDigit(string password) =>
      password.Any(char.IsDigit);

  private bool ContainSpecialCharacter(string password) =>
      password.Any(ch => "!@#$%^&*()_+-=[]{}|;:',.<>?/`~".Contains(ch));

  private bool ContainsUsernameOrEmailPrefix(string password, string email)
  {
    if (string.IsNullOrWhiteSpace(email)) return false;
    var prefix = email.Split('@')[0];
    return password.ToLower().Contains(prefix.ToLower());
  }

  private bool NotContainBlacklistedWords(string password)
  {
    var blacklist = new[] { "password", "admin", "qwerty", "123456", "letmein" };
    return !blacklist.Any(bad => password.ToLower().Contains(bad));
  }
}
