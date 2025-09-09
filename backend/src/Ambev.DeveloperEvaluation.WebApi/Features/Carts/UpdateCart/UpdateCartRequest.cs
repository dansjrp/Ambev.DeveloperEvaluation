using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

public class UpdateCartRequest
{
    [Required]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    [Required]
    public List<CartItemRequest> Items { get; set; } = new();
}

public class CartItemRequest
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
}
