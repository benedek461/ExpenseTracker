using ExpenseTracker.Server.Dtos;
using FluentValidation;

namespace ExpenseTracker.Server.Validators
{
    public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionDto>
    {
        public UpdateTransactionValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                    .WithMessage("Amount must be greater than zero!");
        }
    }
}
