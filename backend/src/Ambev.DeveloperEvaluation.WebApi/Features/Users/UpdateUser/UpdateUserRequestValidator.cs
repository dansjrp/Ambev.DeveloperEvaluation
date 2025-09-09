using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O Id do usuário é obrigatório.");
        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("O primeiro nome é obrigatório.")
            .MaximumLength(50).WithMessage("O primeiro nome deve ter no máximo 50 caracteres.");
        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("O sobrenome é obrigatório.")
            .MaximumLength(50).WithMessage("O sobrenome deve ter no máximo 50 caracteres.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado não é válido.");
        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.");
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("O papel do usuário é obrigatório.");
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("O status do usuário é obrigatório.");
        RuleFor(x => x.Address)
            .NotNull().WithMessage("O endereço é obrigatório.");
        // Adicione outras validações conforme necessário
    }
}
