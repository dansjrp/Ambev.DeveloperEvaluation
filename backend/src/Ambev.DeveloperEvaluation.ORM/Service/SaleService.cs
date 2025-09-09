using Ambev.DeveloperEvaluation.Domain.Services;
using System;

namespace Ambev.DeveloperEvaluation.ORM.Service
{
    public class SaleService : ISaleService
    {
        public decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            if (quantity < 1)
                throw new ArgumentException("Quantity must be greater than zero.");
            if (quantity > 20)
                throw new InvalidOperationException("Cannot sell more than 20 identical items.");
            if (quantity < 4)
                return 0m;
            decimal total = quantity * unitPrice;
            if (quantity >= 10 && quantity <= 20)
                return total * 0.20m;
            if (quantity >= 4 && quantity < 10)
                return total * 0.10m;
            return 0m;
        }
    }
}
