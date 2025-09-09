using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _context;
    public ProductRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Products.FindAsync(new object[] { id }, cancellationToken);

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Products.ToListAsync(cancellationToken);

    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? order, CancellationToken cancellationToken)
    {
        var query = _context.Products.AsNoTracking();

        // Ordering
        if (!string.IsNullOrWhiteSpace(order))
        {
            foreach (var ord in order.Split(','))
            {
                var parts = ord.Trim().Split(' ');
                var prop = parts[0];
                var dir = parts.Length > 1 ? parts[1].ToLower() : "asc";
                if (prop == "title")
                    query = dir == "desc" ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title);
                else if (prop == "price")
                    query = dir == "desc" ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                else if (prop == "category")
                    query = dir == "desc" ? query.OrderByDescending(p => p.Category) : query.OrderBy(p => p.Category);
                // Adicione outros campos conforme necessário
            }
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (products, totalCount);
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedByCategoryAsync(string category, int page, int pageSize, string? order, CancellationToken cancellationToken)
    {
        var query = _context.Products.AsNoTracking().Where(p => p.Category == category);

        // Ordering
        if (!string.IsNullOrWhiteSpace(order))
        {
            foreach (var ord in order.Split(','))
            {
                var parts = ord.Trim().Split(' ');
                var prop = parts[0];
                var dir = parts.Length > 1 ? parts[1].ToLower() : "asc";
                if (prop == "title")
                    query = dir == "desc" ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title);
                else if (prop == "price")
                    query = dir == "desc" ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                else if (prop == "category")
                    query = dir == "desc" ? query.OrderByDescending(p => p.Category) : query.OrderBy(p => p.Category);
                // Adicione outros campos conforme necessário
            }
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (products, totalCount);
    }

    public async Task<List<string>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        => await _context.Products.Select(p => p.Category).Distinct().ToListAsync(cancellationToken);

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        return false;
    }
}
