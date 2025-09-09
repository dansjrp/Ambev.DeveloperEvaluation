using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IProductRepository
{
    /// <summary>
    /// Retorna os IDs dos produtos existentes a partir de uma lista de IDs informada.
    /// </summary>
    Task<List<Guid>> GetExistingProductIdsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? order, CancellationToken cancellationToken);
    Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedByCategoryAsync(string category, int page, int pageSize, string? order, CancellationToken cancellationToken);
    Task<List<string>> GetAllCategoriesAsync(CancellationToken cancellationToken);
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken);
    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
