using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Email).SetValidator(new EmailValidator());

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username must be at most 50 characters.")
            .Matches(@"^(?!\d+$).*").WithMessage("Username cannot be only numbers.");

        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name is required.");
        RuleFor(x => x.Name.Firstname)
            .NotEmpty().WithMessage("Firstname is required.");
        RuleFor(x => x.Name.Lastname)
            .NotEmpty().WithMessage("Lastname is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .Matches(@"^\(\d{2}\) \d{5}-\d{4}$").WithMessage("Phone must be in format (XX) XXXXX-XXXX.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches(@"[A-Z]").WithMessage("Password must contain an uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain a lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain a number.")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain a special character.");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Invalid role.")
            .Must(role => role != UserRole.None).WithMessage("Role cannot be None.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status.")
            .Must(status => status != UserStatus.Unknown).WithMessage("Status cannot be Unknown.");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Address is required.");
        RuleFor(x => x.Address.City)
            .NotEmpty().WithMessage("City is required.");
        RuleFor(x => x.Address.Street)
            .NotEmpty().WithMessage("Street is required.");
        RuleFor(x => x.Address.Number)
            .GreaterThan(0).WithMessage("Number must be greater than 0.");
        RuleFor(x => x.Address.Zipcode)
            .NotEmpty().WithMessage("Zipcode is required.");
        RuleFor(x => x.Address.Geolocation)
            .NotNull().WithMessage("Geolocation is required.");
        RuleFor(x => x.Address.Geolocation.Lat)
            .NotEmpty().WithMessage("Latitude is required.");
    RuleFor(x => x.Address.Geolocation.Long)
        .NotEmpty().WithMessage("Longitude is required.");
    }
}
