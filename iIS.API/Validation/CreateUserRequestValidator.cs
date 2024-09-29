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
        }
    }
}