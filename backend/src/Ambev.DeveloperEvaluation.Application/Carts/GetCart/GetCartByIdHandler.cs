using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartByIdHandler : IRequestHandler<GetCartByIdCommand, Cart>
{
    private readonly ICartRepository _cartRepository;
    public GetCartByIdHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }
    public async Task<Cart> Handle(GetCartByIdCommand request, CancellationToken cancellationToken)
    {
        return await _cartRepository.GetByIdAsync(request.Id);
    }
}
