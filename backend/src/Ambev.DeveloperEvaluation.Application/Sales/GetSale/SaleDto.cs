using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public string Branch { get; set; }
        public decimal Total { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();

        public static SaleDto FromEntity(Sale sale)
        {
            return new SaleDto
            {
                Id = sale.Id,
                Number = sale.Number,
                Date = sale.Date,
                UserId = sale.UserId,
                Branch = sale.Branch,
                Total = sale.Total,
                Items = sale.SaleItems != null
                    ? new List<SaleItemDto>(sale.SaleItems.Select(si => SaleItemDto.FromEntity(si)))
                    : new List<SaleItemDto>()
            };
        }
    }

    public class SaleItemDto
    {
        public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discounts { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Cancelled { get; set; }

        public static SaleItemDto FromEntity(SaleItem item)
        {
            // Usa navegação para Product
            return new SaleItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product?.Title ?? string.Empty,
                Quantity = item.Quantity,
                Price = item.Price,
                Discounts = item.Discounts,
                TotalPrice = item.TotalPrice,
                Cancelled = item.Cancelled
            };
        }
    }
}
