using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

public class UpdateCartRequest
{
    [Required]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    [Required]
    public List<CartItemRequest> Items { get; set; } = new();
}
