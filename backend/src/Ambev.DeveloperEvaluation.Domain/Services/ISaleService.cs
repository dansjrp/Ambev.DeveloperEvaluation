using System;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface ISaleService
    {
        decimal CalculateDiscount(int quantity, decimal unitPrice);
    }
}
