using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(Guid id, params string[] includes);
    Task<(IEnumerable<Cart> Carts, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? orderBy);
    Task AddAsync(Cart cart);
    Task UpdateAsync(Cart cart);
    Task<bool> DeleteAsync(Guid id);
}
