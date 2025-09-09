using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<UpdateProductCommand> _validator;

    public UpdateProductHandler(IProductRepository productRepository, IValidator<UpdateProductCommand> validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = new Product
        {
            Id = request.Id,
            Title = request.Title ?? string.Empty,
            Description = request.Description ?? string.Empty,
            Price = request.Price ?? 0,
            Category = request.Category ?? string.Empty,
            Image = request.Image ?? string.Empty,
            Rating = request.Rating ?? new Rating()
        };
        return await _productRepository.UpdateAsync(product, cancellationToken);
    }
}
