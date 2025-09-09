using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductHandler : IRequestHandler<GetProductCommand, Product>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<GetProductCommand> _validator;

    public GetProductHandler(IProductRepository productRepository, IValidator<GetProductCommand> validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<Product> Handle(GetProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {request.Id} not found");
        return product;
    }
}
