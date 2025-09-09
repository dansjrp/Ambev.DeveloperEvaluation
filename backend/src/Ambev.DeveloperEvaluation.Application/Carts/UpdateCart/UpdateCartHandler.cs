using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, Cart>
{
    private readonly ICartRepository _cartRepository;
    private readonly FluentValidation.IValidator<UpdateCartCommand> _validator;
    private readonly IProductRepository _productRepository;
    public UpdateCartHandler(ICartRepository cartRepository, FluentValidation.IValidator<UpdateCartCommand> validator, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _validator = validator;
        _productRepository = productRepository;
    }
    public async Task<Cart> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var cart = await _cartRepository.GetByIdAsync(request.Id);
        if (cart == null) return null;
        
        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var existingProductIds = await _productRepository.GetExistingProductIdsAsync(productIds, cancellationToken);
        var notFoundIds = productIds.Except(existingProductIds).ToList();

        if (notFoundIds.Any())
            throw new KeyNotFoundException($"Produtos não encontrados: {string.Join(", ", notFoundIds)}");

        cart.Products = request.Items.ConvertAll(i => new CartProduct { ProductId = i.ProductId, Quantity = i.Quantity });
        cart.Date = DateTime.UtcNow;
        
        await _cartRepository.UpdateAsync(cart);
        return cart;
    }
}
