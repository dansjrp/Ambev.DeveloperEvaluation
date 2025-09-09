
using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Representa um carrinho de compras.
/// </summary>
public class Cart : BaseEntity
{
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProduct> Products { get; set; } = new List<CartProduct>();
}

/// <summary>
/// Representa um produto dentro do carrinho.
/// </summary>
public class CartProduct
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
