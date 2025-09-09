using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
    RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.Price).NotNull().GreaterThan(0);
    RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Image).NotEmpty().MaximumLength(250);
    RuleFor(x => x.Rating).NotNull();
    }
}
