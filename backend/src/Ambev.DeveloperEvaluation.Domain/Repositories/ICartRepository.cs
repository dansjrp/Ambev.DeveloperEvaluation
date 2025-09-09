using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(int id);
    Task<IEnumerable<Cart>> GetAllAsync();
    Task AddAsync(Cart cart);
    Task UpdateAsync(Cart cart);
    Task DeleteAsync(int id);
}
