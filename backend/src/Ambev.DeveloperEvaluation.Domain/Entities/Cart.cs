
using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Representa um carrinho de compras.
/// </summary>
public class Cart : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime Date { get; set; }
    public List<CartProduct> Products { get; set; } = new List<CartProduct>();
}

/// <summary>
/// Representa um produto dentro do carrinho.
/// </summary>
public class CartProduct
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
}
