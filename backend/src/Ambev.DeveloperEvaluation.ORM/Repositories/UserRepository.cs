using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IUserRepository using Entity Framework Core
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of UserRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public UserRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new user in the database
    /// </summary>
    /// <param name="user">The user to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user</returns>
    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    /// <summary>
    /// Retrieves a user by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, null otherwise</returns>
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a user by their email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user if found, null otherwise</returns>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>
    /// Deletes a user from the database
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the user was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Retorna uma lista paginada de usuários, ordenada conforme parâmetros informados.
    /// </summary>
    /// <param name="page">Número da página (inicia em 1).</param>
    /// <param name="size">Quantidade de itens por página.</param>
    /// <param name="order">
    /// String de ordenação, exemplo: "username asc, email desc".
    /// Campos suportados: username, email.
    /// </param>
    /// <param name="cancellationToken">Token de cancelamento da operação.</param>
    /// <returns>
    /// Uma tupla contendo a lista de usuários da página e o total de registros.
    /// </returns>
    public async Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedAsync(int page, int size, string? order, CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsNoTracking();

        // Ordering
        if (!string.IsNullOrWhiteSpace(order))
        {
            foreach (var ord in order.Split(','))
            {
                var parts = ord.Trim().Split(' ');
                var prop = parts[0];
                var dir = parts.Length > 1 ? parts[1].ToLower() : "asc";
                if (prop == "username")
                    query = dir == "desc" ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username);
                else if (prop == "email")
                    query = dir == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email);
                // Adicione outros campos conforme necessário
            }
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var users = await query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);
        return (users, totalCount);
    }
}
