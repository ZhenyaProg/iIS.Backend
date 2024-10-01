using FluentValidation;
using iIS.API.Contracts;

namespace iIS.API.Validation
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.LoginType)
                .NotEmpty()
                .Must(loginType => loginType is "un" or "email");

            RuleFor(request => request.Password)
                .NotEmpty();

            When(request => request.LoginType == "un", () =>
                RuleFor(request => request.Login).NotEmpty());

            When(request => request.LoginType == "email", () =>
                RuleFor(request => request.Login).NotEmpty().EmailAddress());
        }
    }
}