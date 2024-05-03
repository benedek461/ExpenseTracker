using ExpenseTracker.Server.Dtos;
using FluentValidation;

namespace ExpenseTracker.Server.Validators
{
    public class CreateTransactionValidator: AbstractValidator<CreateTransactionDto>
    {
        public CreateTransactionValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty()
                    .WithMessage("Amount is empty!")
                .NotNull()
                    .WithMessage("Amount is null!")
                .GreaterThan(0)
                    .WithMessage("Amount must be greater than zero!");
        }
    }
}
