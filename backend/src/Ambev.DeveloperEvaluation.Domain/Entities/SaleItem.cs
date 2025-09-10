using System;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    {
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discounts { get; set; }
    public decimal TotalPrice { get; set; }
    public bool Cancelled { get; set; } // true para cancelado, false para não cancelado
    public required Sale Sale { get; set; }
    public Product Product { get; set; } = null!;
    }
}
