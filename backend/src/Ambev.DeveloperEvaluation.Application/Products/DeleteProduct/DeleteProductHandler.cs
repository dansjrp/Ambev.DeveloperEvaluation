using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<DeleteProductCommand> _validator;

    public DeleteProductHandler(IProductRepository productRepository, IValidator<DeleteProductCommand> validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _productRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
