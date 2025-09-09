using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CloseCart
{
    public class CloseCartResponse
    {
        public Guid SaleId { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public List<CloseCartSaleItemResponse> Items { get; set; } = new();
    }

    public class CloseCartSaleItemResponse
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discounts { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Cancelled { get; set; }
    }
}
