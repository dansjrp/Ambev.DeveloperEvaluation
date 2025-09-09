using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    RuleFor(x => x.Price).NotNull().GreaterThan(0);
    RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
    RuleFor(x => x.Image).NotEmpty().MaximumLength(250);
    RuleFor(x => x.Rating).NotNull();
    }
}
