using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required")
            .Must(role => role == "Customer" || role == "Seller" || role == "Admin")
            .WithMessage("Role must be Customer, Seller, or Admin");

        When(x => x.Role == "Customer", () =>
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required for customer");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required for customer");
        });

        When(x => x.Role == "Seller", () =>
        {
            RuleFor(x => x.StoreName).NotEmpty().WithMessage("Store name is required for seller");
        });
    }
}
