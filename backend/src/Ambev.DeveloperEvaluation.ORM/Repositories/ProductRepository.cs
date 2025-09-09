using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.ORM.Extension;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository : IProductRepository
{


    public async Task<List<Guid>> GetExistingProductIdsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);
    }
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
        var query = _context.Products.AsQueryable();
        // Ordenação dinâmica (exemplo: "title asc,price desc")
        if (!string.IsNullOrEmpty(order))
        {
            foreach (var ord in order.Split(','))
            {
                var parts = ord.Trim().Split(' ');
                var property = parts[0];
                var direction = parts.Length > 1 && parts[1].ToLower() == "desc" ? false : true;
                query = direction ? query.OrderByDynamic(property) : query.OrderByDynamicDescending(property);
            }
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (products, totalCount);
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedByCategoryAsync(string category, int page, int pageSize, string? order, CancellationToken cancellationToken)
    {
        var query = _context.Products.AsQueryable().Where(p => p.Category == category);
        // Ordenação dinâmica (exemplo: "title asc,price desc")
        if (!string.IsNullOrEmpty(order))
        {
            foreach (var ord in order.Split(','))
            {
                var parts = ord.Trim().Split(' ');
                var property = parts[0];
                var direction = parts.Length > 1 && parts[1].ToLower() == "desc" ? false : true;
                query = direction ? query.OrderByDynamic(property) : query.OrderByDynamicDescending(property);
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
