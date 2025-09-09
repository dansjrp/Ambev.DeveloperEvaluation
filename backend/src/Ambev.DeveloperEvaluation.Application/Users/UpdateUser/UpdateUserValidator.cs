using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Validador para UpdateUserCommand
/// </summary>
public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Firstname).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Lastname).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Phone).MaximumLength(20);
        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(r => r != "None")
            .WithMessage("O papel do usuário não pode ser 'None'.");
        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(s => s != "Unknown")
            .WithMessage("O status do usuário não pode ser 'Unknown'.");
        RuleFor(x => x.Address).NotNull();
        // Adicione outras validações conforme necessário
    }
}
