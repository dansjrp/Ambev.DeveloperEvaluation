using Ambev.DeveloperEvaluation.ORM.Extension;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CartRepository : ICartRepository
{
    private readonly DefaultContext _context;
    public CartRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByIdAsync(Guid id, params string[] includes)
    {
        var query = _context.Carts.AsQueryable();
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return await query.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<(IEnumerable<Cart> Carts, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? orderBy)
    {
        var query = _context.Carts
                    .Include(c => c.User)
                    .Include(c => c.Products)
                        .ThenInclude(i => i.Product)
                    .AsQueryable();
        // Ordenação dinâmica (exemplo: "UserId asc,Date desc")
        if (!string.IsNullOrEmpty(orderBy))
        {
            foreach (var order in orderBy.Split(','))
            {
                var parts = order.Trim().Split(' ');
                var property = parts[0];
                var direction = parts.Length > 1 && parts[1].ToLower() == "desc" ? false : true;
                query = direction ? query.OrderByDynamic(property) : query.OrderByDynamicDescending(property);
            }
        }
        var totalCount = await query.CountAsync();
        var carts = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (carts, totalCount);
    }

    public async Task AddAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var cart = await _context.Carts.FindAsync(id);
        if (cart != null)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
