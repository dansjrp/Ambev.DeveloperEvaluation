using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;
        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale?> GetByNumberAsync(int number, params string[] includes)
        {
            var query = _context.Sales.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(s => s.Number == number);
        }

        public async Task<Sale> CreateAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<Sale> UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Sale?> GetByIdAsync(Guid saleId, params string[] includes)
        {
            var query = _context.Sales
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
                .AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(s => s.Id == saleId).FirstOrDefaultAsync();
        }

        public async Task<(List<Sale> Sales, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? orderBy)
        {
            var query = _context.Sales.AsQueryable();
            // Ordenação simples por campo
            if (!string.IsNullOrEmpty(orderBy))
            {
                var parts = orderBy.Split(' ');
                var field = parts[0];
                var desc = parts.Length > 1 && parts[1].ToLower() == "desc";
                query = field switch
                {
                    "date" => desc ? query.OrderByDescending(s => s.Date) : query.OrderBy(s => s.Date),
                    "number" => desc ? query.OrderByDescending(s => s.Number) : query.OrderBy(s => s.Number),
                    _ => query
                };
            }
            var totalCount = await query.CountAsync();
            var sales = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (sales, totalCount);
        }
    }
}
