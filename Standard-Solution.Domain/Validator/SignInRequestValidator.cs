using FluentValidation;
using Standard_Solution.Domain.DTOs.Request;

namespace Standard_Solution.Domain.Validator;
public class SignInRequestValidator : AbstractValidator<UserLoginRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}