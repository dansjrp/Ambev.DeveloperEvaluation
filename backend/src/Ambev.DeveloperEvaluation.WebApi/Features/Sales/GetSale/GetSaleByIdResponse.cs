using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public class GetSaleByIdResponse
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public List<GetSaleItemResponse> Items { get; set; } = new();
    }

    public class GetSaleItemResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discounts { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Cancelled { get; set; }
    }
}
