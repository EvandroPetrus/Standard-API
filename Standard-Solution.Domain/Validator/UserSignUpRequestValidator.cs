using FluentValidation;
using Standard_Solution.Domain.DTOs.Request;

namespace Standard_Solution.Domain.Validator;

public class UserSignUpRequestValidator : AbstractValidator<UserSignUpRequest>
{
    public UserSignUpRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 50).WithMessage("Password must be between {MinLength} and {MaxLength} characters long.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");

        RuleFor(x => x.PasswordCheck)
            .Equal(x => x.Password).WithMessage("Passwords must match.");
    }
}
