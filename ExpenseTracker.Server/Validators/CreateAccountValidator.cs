using ExpenseTracker.Server.Dtos;
using FluentValidation;

namespace ExpenseTracker.Server.Validators
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.Balance)
                .NotNull()
                    .WithMessage("Balance is null!")
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Balance must be greater or equal to zero!");
        }
    }
}
