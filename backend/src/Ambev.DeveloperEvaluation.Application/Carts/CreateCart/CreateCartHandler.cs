using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartHandler : IRequestHandler<CreateCartCommand, Cart>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly FluentValidation.IValidator<CreateCartCommand> _validator;
    public CreateCartHandler(ICartRepository cartRepository, IUserRepository userRepository, IProductRepository productRepository, FluentValidation.IValidator<CreateCartCommand> validator)
    {
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _validator = validator;
    }
    public async Task<Cart> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new FluentValidation.ValidationException(validationResult.Errors);

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new KeyNotFoundException($"User with id {request.UserId} not found.");

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with id {item.ProductId} not found.");
        }

        var cart = new Cart
        {
            UserId = request.UserId,
            Date = DateTime.UtcNow,
            Products = request.Items.ConvertAll(i => new CartProduct { ProductId = i.ProductId, Quantity = i.Quantity })
        };
        await _cartRepository.AddAsync(cart);
        return cart;
    }
}
