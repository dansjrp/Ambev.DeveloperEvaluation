using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartsPaginatedHandler : IRequestHandler<GetCartsPaginatedCommand, (IEnumerable<Cart> Carts, int TotalCount)>
{
    private readonly ICartRepository _cartRepository;
    public GetCartsPaginatedHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }
    public async Task<(IEnumerable<Cart> Carts, int TotalCount)> Handle(GetCartsPaginatedCommand request, CancellationToken cancellationToken)
    {
        return await _cartRepository.GetPaginatedAsync(request.Page, request.PageSize, request.OrderBy);
    }
}
