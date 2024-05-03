using ExpenseTracker.Server.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Server.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty()
                .WithMessage("First Name is empty!")
            .NotNull()
                .WithMessage("First Name is null!");

            RuleFor(x => x.LastName)
                .NotEmpty()
                    .WithMessage("Last Name is empty!")
                .NotNull()
                    .WithMessage("Last Name is null!");

            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage("E-mail address is empty!")
                .NotNull()
                    .WithMessage("E-mail address is null!")
                .EmailAddress()
                    .WithMessage("Invalid e-mail address format!");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Password is empty!")
                .NotNull()
                    .WithMessage("Password is null!");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                    .WithMessage("Confirm password is empty!")
                .NotNull()
                    .WithMessage("Confirm password is null!")
                .Equal(x => x.Password)
                    .WithMessage("Passwords does not match!");
        }
    }
}
