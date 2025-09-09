using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartByIdCommand : IRequest<Cart>
{
    public Guid Id { get; set; }
}
