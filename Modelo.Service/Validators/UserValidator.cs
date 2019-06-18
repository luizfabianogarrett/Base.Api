using FluentValidation;
using Modelo.Domain.Entities;
using System;

namespace Modelo.UserServive.Validators
{
    public class UserValidator : AbstractValidator<UserEntity>
    {
        public UserValidator()
        {
            RuleFor(c => c)
                .NotNull()
                .OnAnyFailure(x =>
                {
                    throw new ArgumentNullException("Object cannot be null.");
                });

            RuleFor(c => c.Cpf)
                .NotEmpty().WithMessage("Is required the CPF.")
                .NotNull().WithMessage("Is required the CPF.");

            RuleFor(c => c.BirthDate)
                .NotEmpty().WithMessage("Is required the birth date.")
                .NotNull().WithMessage("Is required the birth date.");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Is required the name.")
                .NotNull().WithMessage("Is required the name.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Is required the email.")
                .NotNull().WithMessage("Is required the email.")
                .EmailAddress().WithMessage("Invalid email.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Is required the password.")
                .NotNull().WithMessage("Is required the password.");
        }
    }
}
