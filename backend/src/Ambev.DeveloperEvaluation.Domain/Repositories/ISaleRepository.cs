using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale?> GetByNumberAsync(int number, params string[] includes);
        Task<Sale> CreateAsync(Sale sale);
        Task<Sale> UpdateAsync(Sale sale);
        Task<bool> DeleteAsync(Guid id);
        Task<Sale?> GetByIdAsync(Guid saleId, params string[] includes);
    }
}
