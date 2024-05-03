using ExpenseTracker.Server.Dtos;
using FluentValidation;

namespace ExpenseTracker.Server.Validators
{
    public class LoginUserValidator: AbstractValidator<LoginDto>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                    .WithMessage("Username is empty!")
                .NotNull()
                    .WithMessage("Username is null!");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Password is empty!")
                .NotNull()
                    .WithMessage("Password is null!");
        }
    }
}
