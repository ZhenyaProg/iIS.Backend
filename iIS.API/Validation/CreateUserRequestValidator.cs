using FluentValidation;
using iIS.API.Contracts;

namespace iIS.API.Validation
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(request => request.UserName)
                .NotEmpty();

            RuleFor(request => request.Password)
                .NotEmpty();

            RuleFor(request => request.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(request => request.BirthDate)
                .NotEmpty()
                .Must((date) =>
                {
                    if (!DateOnly.TryParse(date, out var parsedDate))
                        return false;

                    DateTime now = DateTime.Now;
                    now = now.AddYears(-parsedDate.Year);
                    now = now.AddMonths(-parsedDate.Month);
                    now = now.AddDays(-parsedDate.Day);

                    if (now.Year >= 14)
                        return true;

                    return false;
                });
        }
    }
}