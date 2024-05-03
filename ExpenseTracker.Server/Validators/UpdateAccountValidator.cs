using ExpenseTracker.Server.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Server.Validators
{
    public class UpdateAccountValidator : AbstractValidator<UpdateAccountDto>
    {
        public UpdateAccountValidator()
        {
            RuleFor(x => x.Balance)
                .NotNull()
                    .WithMessage("Balance is null!")
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Balance must be greater or equal to zero!");
        }
    }
}
